using System.Security.Claims;
using IdentityModel;
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
using PJMS.AuthService.Abstractions.Accessories;
using PJMS.AuthService.Abstractions.Commands.Authentication;
using PJMS.AuthService.Abstractions.Commands.Create;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Exceptions;
using PJMS.AuthService.Web.Attributes;
using PJMS.AuthService.Web.Exceptions;

namespace PJMS.AuthService.Web.External.Controllers;

/// <summary>
/// Класс, представляющий контроллер для внешних провайдеров аутентификации.
/// </summary>
[AllowAnonymous]
[SecurityHeaders]
public class ExternalController : Controller
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
    /// Интерфейс службы событий
    /// </summary>
    private readonly IEventService _events;

    /// <summary>
    /// Медиатор
    /// </summary>
    private readonly ISender _mediator;

    /// <summary>
    /// Конструктор класса ExternalController.
    /// </summary>
    /// <param name="signInManager">Менеджер входа в систему.</param>
    /// <param name="interaction">Сервис взаимодействия с Identity Server.</param>
    /// <param name="events">Сервис событий.</param>
    /// <param name="mediator">Медиатор</param>
    public ExternalController(ISender mediator, SignInManager<AppUser> signInManager,
        IIdentityServerInteractionService interaction,
        IEventService events)
    {
        _mediator = mediator;
        _signInManager = signInManager;
        _interaction = interaction;
        _events = events;
    }

    /// <summary>
    /// инициировать двустороннее обращение к внешнему поставщику аутентификации
    /// </summary>
    [HttpGet]
    public IActionResult Challenge(string? provider, string returnUrl = "/")
    {
        // Проверяем, является ли `provider` пустым или `returnUrl` недействительным
        if (string.IsNullOrEmpty(provider)) throw new QueryParameterMissingException(nameof(provider));

        // Создаем URL-адрес для перенаправления на действие "ExternalLoginCallback" контроллера "External" с параметром "ReturnUrl"
        var redirectUrl = Url.Action("ExternalLoginCallback", "External", new { ReturnUrl = returnUrl });

        // Настраиваем свойства аутентификации для внешней аутентификации с использованием `provider` и `redirectUrl`
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

        // Возвращаем результат вызова вызова аутентификации ChallengeResult с указанным `provider` и `properties`
        return new ChallengeResult(provider, properties);
    }

    /// <summary>
    /// Обрабатывает обратный вызов внешней аутентификации.
    /// </summary>
    /// <param name="returnUrl">URL-адрес возврата после успешной аутентификации.</param>
    /// <returns>Результат действия IActionResult.</returns>
    [HttpGet]
    [Authorize(AuthenticationSchemes = "Identity.External")]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "/")
    {
        // Проверяем, находимся ли мы в контексте запроса авторизации.
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

        // Получаем информацию о внешней аутентификации.
        var info = await _signInManager.GetExternalLoginInfoAsync();
        
        // Отчищаем куки данных от внешнего провайдера
        await HttpContext.SignOutAsync(info!.AuthenticationProperties);

        // Создаем переменную для данных пользователя
        AppUser user;
        try
        {
            // Пробуем аутентифицировать пользователя по внешнему логину
            user = await _mediator.Send(new AuthenticateUserByExternalProviderCommand
            {
                // Провайдер аутентификации
                LoginProvider = info.LoginProvider,

                // Ключ провайдера аутентификации
                ProviderKey = info.ProviderKey
            });
        }

        // Если пользователь не найден - регистрируем его
        catch (UserNotFoundException)
        {
            // Получаем сервис IRequestCultureFeature
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();

            // Получаем текущую локаль
            var locale = requestCulture!.RequestCulture.UICulture.Name.GetLocalization();

            // Отправляем команду на создание пользователя по данным внешнего логина
            user = await _mediator.Send(new CreateUserExternalCommand
            {
                // Данные от внешнего провайдера
                LoginInfo = info,

                // Локаль пользователя
                Locale = locale
            });
        }

        catch (TwoFactorRequiredException ex)
        {
            // Получаем, был ли запомнен пользователь системой 2FA
            var isRemebered = await _signInManager.IsTwoFactorClientRememberedAsync(ex.User);

            // Если пользователь был запомнен
            if (isRemebered)
            {
                // Устанавливаем пользователя из исключения и прерываем обработку исключения (так как пользователь может быть авторизован без 2fa)
                user = ex.User;
            }
            else
            {
                // Формируем объект ClaimsIdentity на основе схемы 2FA
                var identity = new ClaimsIdentity(IdentityConstants.TwoFactorUserIdScheme);

                // Добавляем новый Claim на основе имени пользователя
                identity.AddClaim(new Claim(JwtClaimTypes.Subject, ex.User.Id.ToString()));
                
                // Добавляем новый Claim на основе idp
                identity.AddClaim(new Claim(JwtClaimTypes.IdentityProvider, info.LoginProvider));

                // Осуществляем вход пользователя по схеме 2FA 
                await HttpContext.SignInAsync(IdentityConstants.TwoFactorUserIdScheme, new ClaimsPrincipal(identity));

                // Перенаправляем пользователя на страницу прохождения 2FA
                return RedirectToAction("LoginTwoStep", "TwoFactor",
                    new { returnUrl, rememberMe = true });
            }
        }

        // Выполняем вход пользователя через внешнюю аутентификацию.
        await SignInExternal(user, info, context);

        // Перенаправляем по url возврата
        return Redirect(returnUrl);
    }

    /// <summary>
    /// Асинхронный метод для входа через внешний провайдер аутентификации.
    /// </summary>
    /// <param name="user">Пользователь</param>
    /// <param name="info">Информация о внешнем провайдере аутентификации.</param>
    /// <param name="context">Контекст авторизации.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    private async Task SignInExternal(AppUser user, UserLoginInfo info, AuthorizationRequest? context)
    {
        // Создаем принципал пользователя.
        var principal = await _signInManager.CreateUserPrincipalAsync(user);

        // Создаем объект IdentityServerUser для входа пользователя.
        var isUser = new IdentityServerUser(user.Id.ToString())
        {
            IdentityProvider = info.LoginProvider,
            AdditionalClaims = principal.Claims.ToArray()
        };

        // Выполняем асинхронный вход пользователя.
        await HttpContext.SignInAsync(isUser, new AuthenticationProperties { IsPersistent = true });

        // Генерируем событие успешного входа пользователя.
        await _events.RaiseAsync(new UserLoginSuccessEvent(info.LoginProvider, info.ProviderKey,
            user.Id.ToString(), user.UserName, true, context?.Client.ClientId));
    }
}