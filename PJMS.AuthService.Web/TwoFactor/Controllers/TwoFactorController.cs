using System.Security.Claims;
using System.Web;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PJMS.AuthService.Abstractions.Commands.Authentication;
using PJMS.AuthService.Abstractions.Commands.TwoFactor;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Abstractions.Queries;
using PJMS.AuthService.Web.Attributes;
using PJMS.AuthService.Web.TwoFactor.InputModels;
using PJMS.AuthService.Web.TwoFactor.ViewModels;

namespace PJMS.AuthService.Web.TwoFactor.Controllers;

/// <summary>
/// Контроллер для изменения настроек аккаунта
/// </summary>
[SecurityHeaders]
public class TwoFactorController : Controller
{
    /// <summary>
    /// Медиатор
    /// </summary>
    private readonly ISender _mediator;

    /// <summary>
    /// Предоставляет API для входа пользователя.
    /// </summary>
    private readonly SignInManager<AppUser> _signInManager;

    /// <summary>
    /// Предоставляет услуги, используемые пользовательским интерфейсом для связи с IdentityServer.
    /// </summary>
    private readonly IIdentityServerInteractionService _interaction;

    /// <summary>
    /// Интерфейс службы событий
    /// </summary>
    private readonly IEventService _events;

    /// <summary>
    /// Локализатор
    /// </summary>
    private readonly IStringLocalizer<TwoFactorController> _localizer;

    /// <summary>
    /// Конструктор контроллера для прохождения аутентификации.
    /// </summary>
    /// <param name="mediator">Медиатор</param>
    /// <param name="signInManager">Предоставляет API для входа пользователя.</param>
    /// <param name="localizer">Локализатор</param>
    /// <param name="interaction">Предоставляет услуги, используемые пользовательским интерфейсом для связи с IdentityServer</param>
    /// <param name="events">Интерфейс службы событий</param>
    public TwoFactorController(ISender mediator, SignInManager<AppUser> signInManager,
        IStringLocalizer<TwoFactorController> localizer, IIdentityServerInteractionService interaction,
        IEventService events)
    {
        _mediator = mediator;
        _signInManager = signInManager;
        _localizer = localizer;
        _interaction = interaction;
        _events = events;
    }

    /// <summary>
    /// Точка входа на страницу подключения 2FA
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Setup()
    {
        // Создаем вью-модель подключения 2FA
        var model = await BuildSetupViewModelAsync();

        // возвращаем view
        return View(model);
    }

    /// <summary>
    /// Обработка подключения 2FA
    /// </summary>
    /// <param name="model">Модель подключения 2FA</param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> VerifySetup(SetupTwoFactorInputModel model)
    {
        // Если модель не валидна
        if (!ModelState.IsValid)
        {
            // строим заного модель представления
            var viewModel = await BuildSetupViewModelAsync();

            // передаем в модель введенный ранее код
            viewModel.Code = model.Code;

            // возвращаем представление
            return View("Setup", viewModel);
        }

        try
        {
            // попытка подключения 2FA
            var codes = await _mediator.Send(new VerifySetupTwoFactorTokenCommand
            {
                UserId = User.Id(),
                Code = model.Code!
            });

            // перенаправляем по url возврата
            return View("VerifySetup", new RecoveryCodesViewModel
            {
                RecoveryCodes = codes
            });
        }
        catch (InvalidCodeException)
        {
            // Добавляем локализованную ошибку в модель
            ModelState.AddModelError(string.Empty, _localizer["InvalidCode"]);

            // строим заного модель представления
            var viewModel = await BuildSetupViewModelAsync();

            // передаем в модель введенный ранее код
            viewModel.Code = model.Code;

            // возвращаем представление
            return View("Setup", viewModel);
        }
    }

    /// <summary>
    /// Точка входа на прохождение 2FA
    /// </summary>
    /// <param name="rememberMe">Флаг для запоминания пользователя</param>
    /// <param name="returnUrl">Адрес url возврата</param>
    [HttpGet]
    [Authorize(AuthenticationSchemes = "Identity.TwoFactorUserId")]
    public async Task<IActionResult> LoginTwoStep(bool rememberMe, string returnUrl = "/")
    {
        // Возвращаем представление пользователю
        return View(await BuildLoginTwoStepViewModelAsync(rememberMe, returnUrl, CodeType.Authenticator));
    }

    /// <summary>
    /// Обработка прохождения 2FA
    /// </summary>
    /// <param name="model">Модель прохождения 2FA</param>
    [HttpPost]
    [Authorize(AuthenticationSchemes = "Identity.TwoFactorUserId")]
    public async Task<IActionResult> LoginTwoStep(LoginTwoStepInputModel model)
    {
        // Устанавливаем в строку запроса закодированную returnUrl, чтоб при изменении локали открылась корректная ссылка (смотреть _Culture.cshtml)
        HttpContext.Request.QueryString = new QueryString("?ReturnUrl=" + HttpUtility.UrlEncode(model.ReturnUrl));

        // Если модель не валидна
        if (!ModelState.IsValid)
        {
            // Очищаем список ошибок модели
            ModelState.Clear();
            
            // Заного формируем представление и возвращаем пользователю
            return View(await BuildLoginTwoStepViewModelAsync(model.RememberMe, model.ReturnUrl, model.CodeType));
        }

        // Получаем провайдер из контекста запроса
        var loginProvider = User.FindFirstValue(JwtClaimTypes.IdentityProvider);

        try
        {
            // Отправляем команду на прохождение 2FA
            var user = await _mediator.Send(new AuthenticateTwoFactorCommand
            {
                Code = model.Code!,
                UserId = User.Id(),
                Type = model.CodeType
            });

            // Аутентифицируем пользователя
            await TwoFactorAuthenticateAsync(loginProvider, user, model.RememberMe);

            // Удаляем куки с идентификатором пользователя
            await HttpContext.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);

            // если стоит флаг о запоминании пользователя
            if (model.RememberMe)
            {
                // устанавливаем куки
                await _signInManager.RememberTwoFactorClientAsync(user);
            }

            // Проверяем, находимся ли мы в контексте запроса авторизации
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // Инициализируем событие об успешном входе пользователя
            await _events.RaiseAsync(new UserLoginSuccessEvent(user.Email, user.Id.ToString(), user.UserName,
                clientId: context?.Client.ClientId));


            // Перенаправляем на адрес возврата
            return Redirect(model.ReturnUrl);
        }
        catch (InvalidCodeException)
        {
            ModelState.Clear();

            // // Инициализируем событие об неуспешном входе пользователя
            // await _events.RaiseAsync(new UserLoginFailureEvent(model.Email,
            //     "Invalid credentials", clientId: context?.Client.ClientId));


            // Добавляем локализованную ошибку в модель
            ModelState.AddModelError(string.Empty, _localizer["InvalidCode"]);

            // Заного формируем представление и возвращаем пользователю
            return View(await BuildLoginTwoStepViewModelAsync(model.RememberMe, model.ReturnUrl, model.CodeType));
        }
    }

    /// <summary>
    /// Метод отправляет пользователю код для прохождения 2FA на почту
    /// </summary>
    [Authorize(AuthenticationSchemes = "Identity.TwoFactorUserId, Identity.Application")]
    public async Task<IActionResult> RequestCodeEmail()
    {
        try
        {
            // Отправляем команду на отправку письма с кодом пользователю
            await _mediator.Send(new RequestTwoFactorCodeEmailCommand { UserId = User.Id() });
        }
        catch
        {
            // Возвращаем пустой ответ с кодом 400
            return BadRequest();
        }

        // Возвращаем пустой ответ с кодом 200
        return Ok();
    }

    
    /// <summary>
    /// Точка входа на сброс 2FA
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Reset()
    {
        // Возвращаем представление сброса 2FA
        return View(await BuildResetViewModelAsync(CodeType.Authenticator));
    }

    /// <summary>
    /// Обработка сброса 2FA
    /// </summary>
    /// <param name="model">Модель сброса 2FA</param>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Reset(TwoFactorAuthenticateInputModel model)
    {
        // Если модель не валидна
        if (!ModelState.IsValid)
        {
            // Очищаем список ошибок модели
            ModelState.Clear();
            
            // Заного формируем представление и возвращаем пользователю
            return View(await BuildResetViewModelAsync(model.CodeType));
        }

        try
        {
            // Отправляем команду на сброс 2FA
            var user = await _mediator.Send(new ResetTwoFactorCommand
            {
                UserId = User.Id(),
                Code = model.Code!,
                Type = model.CodeType
            });

            // обновляем вход пользователя, чтобы он остался аутентифицирован после сброса аутентификатора
            await _signInManager.RefreshSignInAsync(user);

            // Перенаправляем пользователя назад в настройки
            return RedirectToAction("Index", "Settings");
        }
        catch (InvalidCodeException)
        {
            // Добавляем локализованную ошибку в модель
            ModelState.AddModelError(string.Empty, _localizer["InvalidCode"]);

            // Заново формируем представление и возвращаем пользователю
            return View(await BuildResetViewModelAsync(model.CodeType));
        }
    }


    /*****************************************/
    /* Вспомогательные методы для AccountController */
    /*****************************************/

    /// <summary>
    /// Метод формирует модель представления для подключения 2FA
    /// </summary>
    /// <returns>Модель представления подключения 2FA</returns>
    private async Task<SetupTwoFactorViewModel> BuildSetupViewModelAsync()
    {
        // Отправляем команду на получение аутентификатора и добавления его пользователю
        var result = await _mediator.Send(new SetupTwoFactorCommand { UserId = User.Id() });

        // обновляем вход пользователя, чтобы он остался аутентифицирован после получения аутентификатора
        await _signInManager.RefreshSignInAsync(result.user);

        // возвращаем представление для подключения 2FA
        return new SetupTwoFactorViewModel(result.token, result.Item1.Email!, "PJMS");
    }

    /// <summary>
    /// Метод формирует модель представления для аутентификации через 2FA
    /// </summary>
    /// <param name="rememberMe">Необходимо ли запоминать вход через 2fa</param>
    /// <param name="returnUrl">Url возврата</param>
    /// <param name="codeType">Тип генерации кода</param>
    /// <returns>Модель представления для аутентификации через 2FA</returns>
    private async Task<LoginTwoStepViewModel> BuildLoginTwoStepViewModelAsync(bool rememberMe, string returnUrl,
        CodeType codeType)
    {
        // Отправляем команду на получение аутентификатора и добавления его пользователю
        var user = await _mediator.Send(new UserByIdQuery { Id = User.Id() });

        // возвращаем представление для подключения 2FA
        return new LoginTwoStepViewModel
        {
            CodeType = codeType,
            NeedShowEmail = user.EmailConfirmed,
            RememberMe = rememberMe,
            ReturnUrl = returnUrl
        };
    }
    
    /// <summary>
    /// Метод формирует модель представления для сброса 2FA
    /// </summary>
    /// <param name="codeType">Тип генерации кода</param>
    /// <returns></returns>
    private async Task<ResetTwoFactorViewModel> BuildResetViewModelAsync(CodeType codeType)
    {
        // Отправляем команду на получение аутентификатора и добавления его пользователю
        var user = await _mediator.Send(new UserByIdQuery { Id = User.Id() });

        // возвращаем представление для сброса 2FA
        return new ResetTwoFactorViewModel
        {
            CodeType = codeType,
            NeedShowEmail = user.EmailConfirmed
        };
    }

    /// <summary>
    /// Метод аутентификации пользователя через 2FA
    /// </summary>
    /// <param name="loginProvider">Внешний провайдер аутентификации</param>
    /// <param name="user">Пользователь</param>
    /// <param name="rememberMe">Необходимо ли запоминать вход через 2fa</param>
    private async Task TwoFactorAuthenticateAsync(string? loginProvider, AppUser user, bool rememberMe)
    {
        // Если провайдер аутентификаци не был получен
        if (string.IsNullOrEmpty(loginProvider))
        {
            // Осуществляем вход для пользователя
            await _signInManager.SignInAsync(user, rememberMe);
        }
        // Если нет
        else
        {
            // Создаем принципал пользователя.
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            // Создаем объект IdentityServerUser для входа пользователя.
            var isUser = new IdentityServerUser(User.Id().ToString())
            {
                IdentityProvider = loginProvider,
                AdditionalClaims = principal.Claims.ToArray()
            };

            // Выполняем асинхронный вход пользователя.
            await HttpContext.SignInAsync(isUser, new AuthenticationProperties { IsPersistent = rememberMe });
        }
    }
}