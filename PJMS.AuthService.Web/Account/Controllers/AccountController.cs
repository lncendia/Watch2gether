using System.Security.Claims;
using System.Web;
using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PJMS.AuthService.Abstractions.Commands.Authentication;
using PJMS.AuthService.Abstractions.Commands.Password;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Web.Account.InputModels;
using PJMS.AuthService.Web.Account.ViewModels;
using PJMS.AuthService.Web.Attributes;
using PJMS.AuthService.Web.Exceptions;

namespace PJMS.AuthService.Web.Account.Controllers;

/// <summary>
/// Контроллер для прохождения аутентификации.
/// </summary>
[SecurityHeaders]
public class AccountController : Controller
{
    /// <summary>
    /// Предоставляет API для входа пользователя.
    /// </summary>
    private readonly SignInManager<AppUser> _signInManager;

    /// <summary>
    /// Предоставляет услуги, используемые пользовательским интерфейсом для связи с IdentityServer.
    /// </summary>
    private readonly IIdentityServerInteractionService _interaction;

    /// <summary>
    /// Отвечает за управление поддерживаемыми схемами аутентификации.
    /// </summary>
    private readonly IAuthenticationSchemeProvider _schemeProvider;

    /// <summary>
    /// Интерфейс службы событий
    /// </summary>
    private readonly IEventService _events;

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
    /// <param name="interaction">Предоставляет услуги, используемые пользовательским
    /// интерфейсом для связи с IdentityServer.</param>
    /// <param name="schemeProvider">Отвечает за управление поддерживаемыми схемами
    /// аутентификации.</param>
    /// <param name="events">Интерфейс службы событий</param>
    /// <param name="localizer">Локализатор</param>
    public AccountController(IMediator mediator, SignInManager<AppUser> signInManager,
        IIdentityServerInteractionService interaction,
        IAuthenticationSchemeProvider schemeProvider,
        IEventService events, IStringLocalizer<AccountController> localizer)
    {
        _mediator = mediator;
        _signInManager = signInManager;
        _interaction = interaction;
        _schemeProvider = schemeProvider;
        _events = events;
        _localizer = localizer;
    }


    /// <summary>
    /// Точка входа на страницу аутентификации
    /// </summary>
    /// <param name="returnUrl">адрес Url переадресации </param>
    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl = "/")
    {
        // Проверяем, находимся ли мы в контексте запроса авторизации
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

        // создаем вью-модель авторизации
        var model = await BuildLoginViewModelAsync(returnUrl, context);

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
        // Проверяем, находимся ли мы в контексте запроса авторизации
        var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

        // Устанавливаем в строку запроса закодированную returnUrl, чтоб при изменении локали открылась корректная ссылка (смотреть _Culture.cshtml)
        HttpContext.Request.QueryString = new QueryString("?ReturnUrl=" + HttpUtility.UrlEncode(model.ReturnUrl));

        // Если модель не валидна
        if (!ModelState.IsValid)
        {
            // Строим заново модель представления
            var loginViewModel = await BuildLoginViewModelAsync(model, context);

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

            // Инициализируем событие об успешном входе пользователя
            await _events.RaiseAsync(new UserLoginSuccessEvent(user.Email,
                user.Id.ToString(), user.UserName, clientId: context?.Client.ClientId));

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

                    // Инициализируем событие об неуспешном входе пользователя
                    await _events.RaiseAsync(new UserLoginFailureEvent(model.Email,
                        "Invalid credentials", clientId: context?.Client.ClientId));

                    // Добавляем локализованную ошибку в модель
                    ModelState.AddModelError(string.Empty, _localizer["InvalidCredentials"]);
                    break;

                // В случае если исключение ex является UserLockoutException добавляем код ошибки в модель
                case UserLockoutException:

                    // Инициализируем событие об неуспешном входе пользователя
                    await _events.RaiseAsync(new UserLoginFailureEvent(model.Email,
                        "User lockout", clientId: context?.Client.ClientId));

                    // Добавляем локализованную ошибку в модель
                    ModelState.AddModelError(string.Empty, _localizer["UserLockout"]);
                    break;

                case TwoFactorRequiredException tfaException:

                    // Получаем, был ли запомнен пользователь системой 2FA
                    var isRemebered = await _signInManager.IsTwoFactorClientRememberedAsync(tfaException.User);

                    // Если пользователь был запомнен
                    if (isRemebered)
                    {
                        // Устанавливаем пользователю аутентификационные куки
                        await _signInManager.SignInAsync(tfaException.User, model.RememberLogin);

                        // Инициализируем событие об успешном входе пользователя
                        await _events.RaiseAsync(new UserLoginSuccessEvent(tfaException.User.UserName,
                            tfaException.User.Id.ToString(), tfaException.User.UserName,
                            clientId: context?.Client.ClientId));

                        // Перенаправляем по url возврата
                        return Redirect(model.ReturnUrl);
                    }

                    // Формируем объект ClaimsIdentity на основе схемы 2FA
                    var identity = new ClaimsIdentity(IdentityConstants.TwoFactorUserIdScheme);

                    // Добавляем новый Claim на основе имени пользователя
                    identity.AddClaim(new Claim(JwtClaimTypes.Subject, tfaException.User.Id.ToString()));

                    // Осуществляем вход пользователя по схеме 2FA 
                    await HttpContext.SignInAsync(IdentityConstants.TwoFactorUserIdScheme,
                        new ClaimsPrincipal(identity));

                    // Перенаправляем пользователя на страницу прохождения 2FA
                    return RedirectToAction("LoginTwoStep", "TwoFactor",
                        new { returnUrl = model.ReturnUrl, rememberMe = model.RememberLogin });

                // Если исключение ex не является ни одним их типов, то вызываем исключение дальше
                default: throw;
            }

            // Строим заново модель представления
            var loginViewModel = await BuildLoginViewModelAsync(model, context);

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
        return View(new ResetPasswordInputModel { ReturnUrl = returnUrl });
    }

    /// <summary>
    /// Обрабатывает POST-запрос для сброса пароля.
    /// </summary>
    /// <param name="model">Модель ввода для сброса пароля.</param>
    /// <returns>Результат действия после сброса пароля.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RecoverPassword(ResetPasswordInputModel model)
    {
        // Если модель не валида - возвращаем представление
        if (!ModelState.IsValid) return View(model);

        // Устанавливаем в строку запроса закодированную returnUrl, чтоб при изменении локали открылась корректная ссылка (смотреть _Culture.cshtml)
        HttpContext.Request.QueryString = new QueryString("?ReturnUrl=" + HttpUtility.UrlEncode(model.ReturnUrl));

        // Генерируем url для смены пароля
        var url = Url.Action(
            "NewPassword", "Account", new { returnUrl = model.ReturnUrl }, HttpContext.Request.Scheme)!;

        try
        {
            // Отправляем команду на смену пароля
            await _mediator.Send(new RequestRecoverPasswordCommand
            {
                // Почта
                Email = model.Email!,

                // Url смены пароля
                ResetUrl = url
            });
        }
        catch (EmailNotConfirmedException)
        {
            // Если почта не подтверждена, то устанавливаем соответствующее сообщение
            ModelState.AddModelError(string.Empty, _localizer["EmailNotConfirmed"]);
        }

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
        catch (PasswordValidationException ex)
        {
            // Добавляем код(ключ) всех ошибок, содержащихся в passwordValidationException.ValidationErrors
            foreach (var error in ex.ValidationErrors)
            {
                ModelState.AddModelError("", _localizer[error.Key]);
            }
        }

        return View(model);
    }

    /// <summary>
    /// Метод обрабатывает нажатие кнопки "Отмена", производит редирект
    /// </summary>
    /// <param name="returnUrl">адрес Url переадресации </param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Cancel(string returnUrl = "/")
    {
        // Проверяем, находимся ли мы в контексте запроса авторизации
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

        // если у нас нет действительного контекста вызываем исключение
        if (context == null) return Redirect(returnUrl);

        // отправляем в IdentityServer отказ пользователя
        await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

        // Перенаправляем по url возврата
        return Redirect(returnUrl);
    }


    /// <summary>
    /// Показать страницу выхода
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Logout(string? logoutId)
    {
        // Получаем контекст деаутентификации
        var context = await _interaction.GetLogoutContextAsync(logoutId);

        // Строем модель для представления
        var inputModel = new LogoutInputModel { LogoutId = logoutId };

        // Если клиент не требует показать страницу подтверждения выхода
        if (!context.ShowSignoutPrompt)
        {
            // Не показываем страницу подтверждения и сразу обрабатываем выход
            return await Logout(inputModel);
        }

        // Показываем страницу подтверждения выхода
        return View(inputModel);
    }

    /// <summary>
    /// Обработка постбэка страницы выхода
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(LogoutInputModel model)
    {
        // Получаем контекст деаутентификации
        var context = await _interaction.GetLogoutContextAsync(model.LogoutId);

        // Создаем модель представления
        var viewModel = new LoggedOutViewModel
        {
            PostLogoutRedirectUri = context.PostLogoutRedirectUri ?? "/",
            SignOutIframeUrl = context.SignOutIFrameUrl
        };

        // Если пользователь не аутентифицирован
        if (User.Identity?.IsAuthenticated != true)
        {
            // Сразу показываем ему представление
            return View("LoggedOut", viewModel);
        }

        // Получаем внешнюю схему аутентификации с возможностью выхода у аутентифицированного пользователя
        var externalSchemeSupportedSignedOut = await GetUserExternalIdpSupportedSignedOutAsync();

        // Отчищаем cookie аутентификации
        await _signInManager.SignOutAsync();

        // Вызываем событие выхода из системы
        await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
        
        // Проверяем, нужно ли нам инициировать выход из вышестоящего поставщика удостоверений
        if (string.IsNullOrEmpty(externalSchemeSupportedSignedOut)) return View("LoggedOut", viewModel);

        // Создаем обратный URL-адрес, чтобы вышестоящий провайдер перенаправлял нас обратно после выхода пользователя из системы
        var url = Url.Action("Logout", new { logoutId = model.LogoutId });

        // Перенаправляем к внешнему провайдеру для выхода
        return SignOut(new AuthenticationProperties { RedirectUri = url }, externalSchemeSupportedSignedOut);
    }

    /*****************************************/
    /* Вспомогательные методы для AccountController */
    /*****************************************/

    /// <summary>
    /// Создает модель представления входа
    /// </summary>
    /// <param name="returnUrl">url возврата</param>
    /// <param name="context">Контекст авторизации</param>
    /// <returns>Модель представления входа</returns>
    private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl, AuthorizationRequest? context)
    {
        //получаем все схемы, зарегистрированные в приложении
        var allIdentityProviders = await _schemeProvider.GetAllSchemesAsync();

        // Получаем все внешние провайдеры идентификации (oauth схемы)
        var externalIdentityProviders = allIdentityProviders
            .Where(scheme => scheme.IsOauthScheme())
            .Select(x => x.Name);

        // Устанавливаем, что по умолчанию включен локальный провайдер (вход по логину и паролю)
        var enableLocalIdentityProvider = true;

        // Если контекст аутентификации IdentityServer null
        if (context == null)
        {
            // Формируем вью-модель входа в систему
            return new LoginViewModel
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
            if (context.IdP == IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
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
        if (client.IdentityProviderRestrictions.Count != 0)
        {
            // Оставляем только те схемы, которые разрешены
            externalIdentityProviders = externalIdentityProviders.Where(client.IdentityProviderRestrictions.Contains);
        }

        // Устанавливаем, что локальный провайдер может быть задействован только если он включен у клиента
        enableLocalIdentityProvider &= client.EnableLocalLogin;

        // формируем вью-модель входа в систему
        return new LoginViewModel
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
    /// <param name="model">Модель, прилетевшая в контроллер</param>
    /// <param name="context">Контекст авторизации</param>
    /// <returns></returns>
    private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model, AuthorizationRequest? context)
    {
        //формируем вью модель из returnUrl
        var vm = await BuildLoginViewModelAsync(model.ReturnUrl, context);

        //добавляем email из прилетевшей в контроллер модели
        vm.Email = model.Email;

        //добавляем rememberLogin из прилетевшей в контроллер модели
        vm.RememberLogin = model.RememberLogin;

        //добавляем password из прилетевшей в контроллер модели
        vm.Password = model.Password;

        //возвращаем модель
        return vm;
    }

    /// <summary>
    /// Получает внешнюю схему аутентификации пользователя и если она есть, проверяет, поддерживает ли она выход
    /// </summary>
    /// <returns>Внешнюю схему аутентификации пользователя, поддерживающую выход или null</returns>
    private async Task<string?> GetUserExternalIdpSupportedSignedOutAsync()
    {
        // переменная для внешнего провайдера
        string? externalIdp = null;

        // Получение идентификатора провайдера идентичности пользователя, если он есть.
        var idp = User.GetIdentityProvider();

        // Проверка, является ли провайдер идентичности пользователя внешним
        if (idp is null or IdentityServer4.IdentityServerConstants.LocalIdentityProvider) return externalIdp;
        
        // Проверка поддержки провайдером выхода из системы.
        if (await HttpContext.GetSchemeSupportsSignOutAsync(idp))
        {
            // Если провайдер поддерживает, устанавливаем idp
            externalIdp = idp;
        }

        // Возвращаем модель и внешнюю схему аутентификации
        return externalIdp;
    }
}