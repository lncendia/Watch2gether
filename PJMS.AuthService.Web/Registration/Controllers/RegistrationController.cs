using System.Web;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PJMS.AuthService.Abstractions.Accessories;
using PJMS.AuthService.Abstractions.Commands.Create;
using PJMS.AuthService.Abstractions.Commands.Email;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Web.Attributes;
using PJMS.AuthService.Web.Exceptions;
using PJMS.AuthService.Web.Registration.InputModels;
using PJMS.AuthService.Web.Registration.ViewModels;

namespace PJMS.AuthService.Web.Registration.Controllers;

/// <summary>
/// Контроллер для прохождения регистрации.
/// </summary>
[SecurityHeaders]
public class RegistrationController : Controller
{
    /// <summary>
    /// Медиатр
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Предоставляет услуги, используемые пользовательским интерфейсом для связи с IdentityServer.
    /// </summary>
    private readonly IIdentityServerInteractionService _interaction;

    /// <summary>
    /// Интерфейс службы событий
    /// </summary>
    private readonly IEventService _events;

    /// <summary>
    /// Предоставляет API для входа пользователя.
    /// </summary>
    private readonly SignInManager<AppUser> _signInManager;

    /// <summary>
    /// Отвечает за управление поддерживаемыми схемами аутентификации.
    /// </summary>
    private readonly IAuthenticationSchemeProvider _schemeProvider;

    /// <summary>
    /// Локализатор
    /// </summary>
    private readonly IStringLocalizer<RegistrationController> _localizer;

    /// <summary>
    /// Конструктор контроллера для прохождения регистрации.
    /// </summary>
    /// <param name="signInManager">Предоставляет API для входа пользователя</param>
    /// <param name="interaction">Предоставляет услуги, используемые пользовательским интерфейсом для связи с IdentityServer</param>
    /// <param name="schemeProvider">Отвечает за управление поддерживаемыми схемами аутентификации</param>
    /// <param name="events">Интерфейс службы событий</param>
    /// <param name="localizer">Локализатор</param>
    /// <param name="mediator">Медиатр</param>
    public RegistrationController(IIdentityServerInteractionService interaction,
        IAuthenticationSchemeProvider schemeProvider,
        SignInManager<AppUser> signInManager,
        IEventService events, IStringLocalizer<RegistrationController> localizer, IMediator mediator)
    {
        _interaction = interaction;
        _schemeProvider = schemeProvider;
        _signInManager = signInManager;
        _events = events;
        _localizer = localizer;
        _mediator = mediator;
    }

    /// <summary>
    /// Метод отдает View регистрации
    /// </summary>
    /// <param name="returnUrl">Url для возврата</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Registration(string? returnUrl)
    {
        // Проверяем, находимся ли мы в контексте запроса авторизации
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

        // создаем вью-модель регистрации
        var vm = await BuildRegisterViewModelAsync(returnUrl!, context);

        // возвращаем view
        return View(vm);
    }

    /// <summary>
    /// Обработка регистрации пользователя
    /// </summary>
    /// <param name="model">Модель входа в систему</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Registration(RegistrationInputModel model)
    {
        // Получаем контекст запроса аутентификации от IdentityServer
        var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

        // Устанавливаем в строку запроса закодированную returnUrl, чтоб при изменении локали открылась корректная ссылка (смотреть _Culture.cshtml)
        HttpContext.Request.QueryString = new QueryString("?ReturnUrl=" + HttpUtility.UrlEncode(model.ReturnUrl));

        // Если данные не валидны
        if (!ModelState.IsValid)
        {
            // что-то пошло не так, показать форму с ошибкой
            var vm = await BuildRegisterViewModelAsync(model, context);

            //возвращаем модель во вью
            return View(vm);
        }

        // Создает URL-адрес для подтверждения почты
        var callbackUrl = Url.Action("ConfirmEmail", "Registration", null, HttpContext.Request.Scheme)!;

        // Получаем сервис IRequestCultureFeature
        var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();

        // Получаем текущую локаль
        var locale = requestCulture!.RequestCulture.UICulture.Name.GetLocalization();

        try
        {
            // Отправляем команду на создание пользователя
            var user = await _mediator.Send(new CreateUserCommand
            {
                // Почта
                Email = model.Email!,

                // Пароль
                Password = model.Password!,

                // Url подтверждения почты
                ConfirmUrl = callbackUrl,

                // Локаль
                Locale = locale
            });

            //Устанавливаем пользователю аутентификационные куки
            await _signInManager.SignInAsync(user, model.RememberLogin);

            // Инициализируем событие об успешном входе пользователя
            await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName,
                user.Id.ToString(), user.UserName, clientId: context?.Client.ClientId));

            //Делаем редирект
            return Redirect(model.ReturnUrl);
        }
        catch (Exception ex)
        {
            // Проверяем какое исключение мы словили и добавляем в ModelState соответсвующее значение.
            switch (ex)
            {
                //В случае если исключение ex является EmailAlreadyTakenException добавляем код ошибки в модель
                case EmailAlreadyTakenException:
                    ModelState.AddModelError("", _localizer["UserAlreadyExist"]);
                    break;

                //В случае если исключение ex является EmailFormatException добавляем код ошибки в модель
                case EmailFormatException:
                    ModelState.AddModelError("", _localizer["EmailFormatInvalid"]);
                    break;

                //В случае если исключение ex является PasswordValidationException
                //Добавляеем код(ключ) всех ошибок, содержащихся в passwordValidationException.ValidationErrors
                case PasswordValidationException passwordValidationException:
                    foreach (var error in passwordValidationException.ValidationErrors)
                    {
                        ModelState.AddModelError("", _localizer[error.Key]);
                    }

                    break;

                //Если исключение ex не является ни одним их типов, то вызываем исключение дальше
                default: throw;
            }

            // Создаем модель представления регистрации
            var vm = await BuildRegisterViewModelAsync(model, context);

            // Возвращаем представление
            return View(vm);
        }
    }

    /// <summary>
    /// Метод подтверждения email
    /// </summary>
    /// <param name="userId">Id пользователя</param>
    /// <param name="code">Токен для подтверждения email пользователя</param>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(Guid? userId, string? code)
    {
        // проверяем входящие данные
        if (!userId.HasValue) throw new QueryParameterMissingException(nameof(userId));

        // проверяем входящие данные
        if (code == null) throw new QueryParameterMissingException(nameof(code));

        // Верифицируем email
        await _mediator.Send(new VerifyEmailCommand
        {
            // Идентификатор пользователя
            UserId = userId.Value,

            // Код подтверждения
            Code = code
        });

        // Делаем редирект
        return RedirectToAction("EmailConfirmed");
    }
    
    /// <summary>
    /// Возвращает представление для страницы "EmailConfirmed".
    /// </summary>
    /// <returns>Результат действия для страницы "EmailConfirmed".</returns>
    public IActionResult EmailConfirmed() => View();


    /*****************************************/
    /* Вспомогательные методы для RegistrationController */
    /*****************************************/

    /// <summary>
    /// Создает модель представления регистрации
    /// </summary>
    /// <param name="returnUrl">Url для возврата</param>
    /// <param name="context">Контекст авторизации</param>
    /// <returns>Вью-модель регистрации в систему</returns>
    private async Task<RegistrationViewModel> BuildRegisterViewModelAsync(string returnUrl, AuthorizationRequest? context)
    {
        //получаем все схемы, зарегистрированные в приложении
        var allIdentityProviders = await _schemeProvider.GetAllSchemesAsync();

        // Получаем все внешние провайдеры идентификации (oauth схемы)
        var externalIdentityProviders = allIdentityProviders
            .Where(scheme => scheme.IsOauthScheme())
            .Select(x => x.Name);

        // Устанавливаем, что по умолчанию включен локальный провайдер (вход по логину и паролю)
        var enableLocalIdentityProvider = true;

        // Если контекст аутентификации IdentityServer не null
        if (context == null)
        {
            // Формируем вью-модель входа в систему
            return new RegistrationViewModel
            {
                // Url возврата
                ReturnUrl = returnUrl,

                // Флаг, включен ли локальный провайдер идентификации
                EnableLocalLogin = enableLocalIdentityProvider,

                // Массив доступных внешних провайдеров
                ExternalProviders = externalIdentityProviders.ToArray()
            };
        }

        //для удобства запишем клиента в переменную
        var client = context.Client;

        // Если в контексте аутентификации запрошен какой-то конкретный провайдер
        if (context.IdP != null)
        {
            // Если это локальный провайдер (вход с помощью логина и пароля)
            if (context.IdP == IdentityServerConstants.LocalIdentityProvider)
            {
                // Удаляем все внешние провайдеры (так как запрошен локальный)
                externalIdentityProviders = [];
            }
            else
            {
                // Отключаем локальный провайдер, так как запрошен внешний
                enableLocalIdentityProvider = false;

                // Удаляем все провайдеры кроме запрашиваемого
                externalIdentityProviders = externalIdentityProviders.Where(provider => provider == context.IdP);
            }
        }

        // Если есть ограничения на использование внешних схем клиентом
        if (client.IdentityProviderRestrictions.Any())
        {
            // Оставляем только те схемы, которые разрешены
            externalIdentityProviders = externalIdentityProviders.Where(client.IdentityProviderRestrictions.Contains);
        }

        // Устанавливаем, что локальный провайдер может быть задействован только если он включен у клиента
        enableLocalIdentityProvider &= client.EnableLocalLogin;

        // формируем вью-модель входа в систему
        return new RegistrationViewModel
        {
            // Url возврата
            ReturnUrl = returnUrl,

            // Флаг, включен ли локальный провайдер идентификации
            EnableLocalLogin = enableLocalIdentityProvider,

            // Массив доступных внешних провайдеров
            ExternalProviders = externalIdentityProviders.ToArray(),

            // Подсказка для логина
            Email = context.LoginHint
        };
    }


    /// <summary>
    /// Построить асинхронную модель представления входа
    /// </summary>
    /// <param name="model">Модель входа в систему</param>
    /// <param name="context">Контекст авторизации</param>
    /// <returns>Вью-модель входа в систему</returns>
    private async Task<RegistrationViewModel> BuildRegisterViewModelAsync(RegistrationInputModel model,
        AuthorizationRequest? context)
    {
        // Построить асинхронную модель представления входа
        var vm = await BuildRegisterViewModelAsync(model.ReturnUrl, context);

        //устанавливаем прилетевшую в контроллер почту
        vm.Email = model.Email;

        //устанавливаем прилетевший в контроллер пароль
        vm.Password = model.Password;

        //устанавливаем прилетевший в контроллер пароль
        vm.PasswordConfirm = model.PasswordConfirm;

        //возвращаем вью-модель
        return vm;
    }
}