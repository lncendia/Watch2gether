using System.Web;
using AuthService.Application.Abstractions.Commands;
using AuthService.Application.Abstractions.Entities;
using AuthService.Application.Abstractions.Exceptions;
using AuthService.Infrastructure.Web.Exceptions;
using AuthService.Infrastructure.Web.Registration.InputModels;
using AuthService.Infrastructure.Web.Registration.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AuthService.Infrastructure.Web.Registration.Controllers;

/// <summary>
/// Контроллер для прохождения регистрации.
/// </summary>
public class RegistrationController : Controller
{
    /// <summary>
    /// Медиатр
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Предоставляет API для входа пользователя.
    /// </summary>
    private readonly SignInManager<UserData> _signInManager;

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
    /// <param name="schemeProvider">Отвечает за управление поддерживаемыми схемами аутентификации</param>
    /// <param name="localizer">Локализатор</param>
    /// <param name="mediator">Медиатр</param>
    public RegistrationController(IAuthenticationSchemeProvider schemeProvider, SignInManager<UserData> signInManager,
        IStringLocalizer<RegistrationController> localizer, IMediator mediator)
    {
        _schemeProvider = schemeProvider;
        _signInManager = signInManager;
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
        // создаем вью-модель регистрации
        var vm = await BuildRegisterViewModelAsync(returnUrl!);

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
        // Устанавливаем в строку запроса закодированную returnUrl, чтоб при изменении локали открылась корректная ссылка (смотреть _Culture.cshtml)
        HttpContext.Request.QueryString = new QueryString("?ReturnUrl=" + HttpUtility.UrlEncode(model.ReturnUrl));

        // Если данные не валидны
        if (!ModelState.IsValid)
        {
            // что-то пошло не так, показать форму с ошибкой
            var vm = await BuildRegisterViewModelAsync(model);

            //возвращаем модель во вью
            return View(vm);
        }

        // Создает URL-адрес для подтверждения почты
        var callbackUrl = Url.Action("ConfirmEmail", "Registration", null, HttpContext.Request.Scheme)!;

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

                Username = model.Username!
            });

            //Устанавливаем пользователю аутентификационные куки
            await _signInManager.SignInAsync(user, model.RememberLogin);

            //Делаем редирект
            return Redirect(model.ReturnUrl);
        }
        catch (Exception ex)
        {
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

                //В случае если исключение ex является UserNameFormatException добавляем код ошибки в модель
                case UserNameFormatException:
                    ModelState.AddModelError("", _localizer["UserNameFormatInvalid"]);
                    break;
                
                //В случае если исключение ex является UserNameLengthException добавляем код ошибки в модель
                case UserNameLengthException:
                    ModelState.AddModelError("", _localizer["UserNameLengthInvalid"]);
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
            var vm = await BuildRegisterViewModelAsync(model);

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
    public async Task<IActionResult> ConfirmEmail(long? userId, string? code)
    {
        // проверяем входящие данные
        if (!userId.HasValue) throw new QueryParameterMissingException(nameof(userId));

        // проверяем входящие данные
        if (code == null) throw new QueryParameterMissingException(nameof(code));

        // Верифицируем email
        await _mediator.Send(new VerifyUserEmailCommand
        {
            // Идентификатор пользователя
            Id = userId.Value,

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
    /* вспомогательные API для RegistrationController */
    /*****************************************/

    /// <summary>
    /// Создает модель представления регистрации
    /// </summary>
    /// <param name="returnUrl">Url для возврата</param>
    /// <returns>Вью-модель регистрации в систему</returns>
    private async Task<RegistrationViewModel> BuildRegisterViewModelAsync(string returnUrl)
    {
        //получаем все схемы, зарегистрированные в приложении
        var allIdentityProviders = await _schemeProvider.GetAllSchemesAsync();

        // Получаем все внешние провайдеры идентификации (oauth схемы)
        var externalIdentityProviders = allIdentityProviders
            .Where(scheme => scheme.IsOauthScheme())
            .Select(x => x.Name);

        // Формируем вью-модель входа в систему
        return new RegistrationViewModel
        {
            // Url возврата
            ReturnUrl = returnUrl,

            // Массив доступных внешних провайдеров
            ExternalProviders = externalIdentityProviders.ToArray()
        };
    }


    /// <summary>
    /// Построить асинхронную модель представления входа
    /// </summary>
    /// <param name="model">Модель входа в систему</param>
    /// <returns>Вью-модель входа в систему</returns>
    private async Task<RegistrationViewModel> BuildRegisterViewModelAsync(RegistrationInputModel model)
    {
        // Построить асинхронную модель представления входа
        var vm = await BuildRegisterViewModelAsync(model.ReturnUrl);

        //устанавливаем прилетевшую в контроллер почту
        vm.Email = model.Email;

        //устанавливаем прилетевший в контроллер пароль
        vm.Password = model.Password;

        //устанавливаем прилетевшее в контроллер имя
        vm.Username = model.Username;

        //устанавливаем прилетевший в контроллер пароль
        vm.PasswordConfirm = model.PasswordConfirm;

        //возвращаем вью-модель
        return vm;
    }
}