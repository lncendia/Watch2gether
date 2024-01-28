using System.Web;
using AuthService.Application.Abstractions.Commands;
using AuthService.Application.Abstractions.Entities;
using AuthService.Application.Abstractions.Exceptions;
using AuthService.Infrastructure.Web.Account.InputModels;
using AuthService.Infrastructure.Web.Account.ViewModels;
using AuthService.Infrastructure.Web.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AuthService.Infrastructure.Web.Account.Controllers;

/// <summary>
/// Контроллер для прохождения аутентификации.
/// </summary>
public class AccountController : Controller
{
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
    private readonly IStringLocalizer<AccountController> _localizer;

    /// <summary>
    /// Медиатор
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Конструктор контроллера для прохождения аутентификации.
    /// </summary>
    /// <param name="mediator">Медиатр</param>
    /// <param name="signInManager">Предоставляет API для входа пользователя.</param>
    /// <param name="schemeProvider">Отвечает за управление поддерживаемыми схемами
    /// аутентификации.</param>
    /// <param name="localizer">Локализатор</param>
    public AccountController(IMediator mediator, SignInManager<UserData> signInManager,
        IAuthenticationSchemeProvider schemeProvider, IStringLocalizer<AccountController> localizer)
    {
        _mediator = mediator;
        _signInManager = signInManager;
        _schemeProvider = schemeProvider;
        _localizer = localizer;
    }


    /// <summary>
    /// Точка входа на страницу аутентификации
    /// </summary>
    /// <param name="returnUrl">адрес Url переадресации </param>
    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl = "/")
    {
        // создаем вью-модель авторизации
        var model = await BuildLoginViewModelAsync(returnUrl);

        // возвращаем view
        return View(model);
    }

    /// <summary>
    /// Обработка аутентификации
    /// </summary>
    /// <param name="model">Модель входа в систему</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginInputModel model)
    {
        // Устанавливаем в строку запроса закодированную returnUrl, чтоб при изменении локали открылась корректная ссылка (смотреть _Culture.cshtml)
        HttpContext.Request.QueryString = new QueryString("?ReturnUrl=" + HttpUtility.UrlEncode(model.ReturnUrl));

        // Если модель не валидна
        if (!ModelState.IsValid)
        {
            // Строим заново модель представления
            var loginViewModel = await BuildLoginViewModelAsync(model);

            // Возвращаем представление
            return View(loginViewModel);
        }

        try
        {
            // Попытка аутентификации пользователя с использованием введенных учетных данных.
            var user = await _mediator.Send(new AuthenticateUserByPasswordCommand
            {
                Email = model.Email!,
                Password = model.Password!
            });

            // Устанавливаем пользователю аутентификационные куки
            await _signInManager.SignInAsync(user, model.RememberLogin);

            // Перенаправляем по url возврата
            return Redirect(model.ReturnUrl);
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                // В случае если исключение ex является UserNotFoundException добавляем код ошибки в модель
                case UserNotFoundException:

                    // Добавляем локализованную ошибку в модель
                    ModelState.AddModelError(string.Empty, _localizer["UserNotFound"]);
                    break;

                // В случае если исключение ex является InvalidPasswordException добавляем код ошибки в модель
                case InvalidPasswordException:
                    
                    // Добавляем локализованную ошибку в модель
                    ModelState.AddModelError(string.Empty, _localizer["InvalidCredentials"]);
                    break;

                // В случае если исключение ex является UserNotFoundException добавляем код ошибки в модель
                case UserLockoutException:
                    
                    // Добавляем локализованную ошибку в модель
                    ModelState.AddModelError(string.Empty, _localizer["UserLockout"]);
                    break;

                // Если исключение ex не является ни одним их типов, то вызываем исключение дальше
                default: throw;
            }

            // Строим заново модель представления
            var loginViewModel = await BuildLoginViewModelAsync(model);

            // Возвращаем представление
            return View(loginViewModel);
        }
    }

    /// <summary>
    /// Обрабатывает HTTP GET запрос для сброса пароля.
    /// </summary>
    /// <param name="returnUrl">URL возврата.</param>
    /// <returns>Результат действия для сброса пароля.</returns>
    [HttpGet]
    public IActionResult RecoverPassword(string returnUrl = "/")
    {
        // Возвращаем представление с моделью
        return View(new RecoverPasswordInputModel { ReturnUrl = returnUrl });
    }

    /// <summary>
    /// Обрабатывает POST-запрос для сброса пароля.
    /// </summary>
    /// <param name="model">Модель ввода для сброса пароля.</param>
    /// <returns>Результат действия после сброса пароля.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RecoverPassword(RecoverPasswordInputModel model)
    {
        // Если модель не валида - возвращаем представление
        if (!ModelState.IsValid) return View(model);

        // Устанавливаем в строку запроса закодированную returnUrl, чтоб при изменении локали открылась корректная ссылка (смотреть _Culture.cshtml)
        HttpContext.Request.QueryString = new QueryString("?ReturnUrl=" + HttpUtility.UrlEncode(model.ReturnUrl));

        // Генерируем url для смены пароля
        var url = Url.Action(
            "NewPassword", "Account", new { returnUrl = model.ReturnUrl }, HttpContext.Request.Scheme)!;

        // Отправляем команду на смену пароля
        await _mediator.Send(new RequestRecoverPasswordCommand
        {
            // Почта
            Email = model.Email!,

            // Url смены пароля
            ResetUrl = url
        });

        // Перенаправляем на страницу MailSent
        return RedirectToAction("MailSent");
    }

    /// <summary>
    /// Возвращает представление для страницы "MailSent".
    /// </summary>
    /// <returns>Результат действия для страницы "MailSent".</returns>
    public IActionResult MailSent() => View();

    /// <summary>
    /// Обрабатывает HTTP GET запрос для установки нового пароля.
    /// </summary>
    /// <param name="email">Параметр email.</param>
    /// <param name="code">Параметр code.</param>
    /// <param name="returnUrl">URL возврата.</param>
    /// <returns>Результат действия для установки нового пароля.</returns>
    [HttpGet]
    public IActionResult NewPassword(string? email, string? code, string returnUrl = "/")
    {
        // Выбрасывание исключения QueryParameterMissingException, если параметр email отсутствует
        if (string.IsNullOrEmpty(email)) throw new QueryParameterMissingException(nameof(email));

        // Выбрасывание исключения QueryParameterMissingException, если параметр code отсутствует
        if (string.IsNullOrEmpty(code)) throw new QueryParameterMissingException(nameof(code));

        // Возвращаем представление с моделью
        return View(new NewPasswordInputModel { Email = email, Code = code, ReturnUrl = returnUrl });
    }

    /// <summary>
    /// Обрабатывает POST-запрос на сброс нового пароля.
    /// </summary>
    /// <param name="model">Модель ввода для нового пароля.</param>
    /// <returns>Результат действия после сброса пароля.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NewPassword(NewPasswordInputModel model)
    {
        // Устанавливаем в строку запроса закодированную returnUrl, чтоб при изменении локали открылась корректная ссылка (смотреть _Culture.cshtml)
        HttpContext.Request.QueryString =
            new QueryString("?ReturnUrl=" + HttpUtility.UrlEncode(model.ReturnUrl) + "&Email=" +
                            HttpUtility.UrlEncode(model.Email) + "&Code=" + HttpUtility.UrlEncode(model.Code));

        // Если модель не валида - возвращаем представление
        if (!ModelState.IsValid) return View(model);

        try
        {
            // Отправляем команду на сброс пароля и установку нового пароля
            await _mediator.Send(new RecoverPasswordCommand
            {
                // Код сброса пароля
                Code = model.Code!,

                // Почта
                Email = model.Email!,

                // Новый пароль
                NewPassword = model.NewPassword!
            });

            // Перенаправляем пользователя на страницу входа и устанавливаем returnUrl
            return RedirectToAction("Login", new { returnUrl = model.ReturnUrl });
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                //В случае если исключение ex является UserNotFoundException добавляем код ошибки в модель
                case UserNotFoundException:
                    ModelState.AddModelError("", _localizer["UserNotFound"]);
                    break;

                //В случае если исключение ex является PasswordValidationException
                case PasswordValidationException passwordValidationException:

                    // Добавляем код(ключ) всех ошибок, содержащихся в passwordValidationException.ValidationErrors
                    foreach (var error in passwordValidationException.ValidationErrors)
                    {
                        ModelState.AddModelError("", _localizer[error.Key]);
                    }

                    break;

                //Если исключение ex не является ни одним их типов, то вызываем исключение дальше
                default: throw;
            }

            return View(model);
        }
    }

    [Authorize]
    public async Task<IActionResult> Logout(string returnUrl = "/")
    {
        await _signInManager.SignOutAsync();
        return Redirect(returnUrl);
    }


    /*****************************************/
    /* вспомогательные API для AccountController */
    /*****************************************/

    /// <summary>
    /// Создает модель представления входа
    /// </summary>
    /// <param name="returnUrl">url возврата</param>
    /// <returns>Модель представления входа</returns>
    private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
    {
        //получаем все схемы, зарегистрированные в приложении
        var allIdentityProviders = await _schemeProvider.GetAllSchemesAsync();

        // Получаем все внешние провайдеры идентификации (oauth схемы)
        var externalIdentityProviders = allIdentityProviders
            .Where(scheme => scheme.IsOauthScheme())
            .Select(x => x.Name);

        // Формируем вью-модель входа в систему
        return new LoginViewModel
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
    /// <param name="model">Модель, прилетевшая в контроллер</param>
    /// <returns>Модель представления входа</returns>
    private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
    {
        //формируем вью модель из returnUrl
        var vm = await BuildLoginViewModelAsync(model.ReturnUrl);

        //добавляем email из прилетевшей в контроллер модели
        vm.Email = model.Email;

        //добавляем rememberLogin из прилетевшей в контроллер модели
        vm.RememberLogin = model.RememberLogin;

        //добавляем password из прилетевшей в контроллер модели
        vm.Password = model.Password;
        
        //возвращаем модель
        return vm;
    }
}