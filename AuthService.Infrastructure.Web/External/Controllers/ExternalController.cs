using AuthService.Application.Abstractions.Commands;
using AuthService.Application.Abstractions.Entities;
using AuthService.Application.Abstractions.Exceptions;
using AuthService.Infrastructure.Web.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Infrastructure.Web.External.Controllers;

/// <summary>
/// Класс, представляющий контроллер для внешних провайдеров аутентификации.
/// </summary>
/// <param name="signInManager">Менеджер входа в систему.</param>
/// <param name="mediator">Медиатор</param>
[AllowAnonymous]
public class ExternalController(IMediator mediator, SignInManager<UserData> signInManager) : Controller
{
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
        var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

        // Возвращаем результат вызова вызова аутентификации ChallengeResult с указанным `provider` и `properties`
        return new ChallengeResult(provider, properties);
    }

    /// <summary>
    /// Обрабатывает обратный вызов внешней аутентификации.
    /// </summary>
    /// <param name="returnUrl">URL-адрес возврата после успешной аутентификации.</param>
    /// <returns>Результат действия IActionResult.</returns>
    [HttpGet]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "/")
    {
        // Получаем информацию о внешней аутентификации.
        var info = await signInManager.GetExternalLoginInfoAsync();

        // Если информация отсутствует, вызываем исключение.
        if (info == null) throw new ExternalLoginException();

        // Создаем переменную для данных пользователя
        UserData user;
        try
        {
            // Пробуем аутентифицировать пользователя по внешнему логину
            user = await mediator.Send(new AuthenticateUserByExternalProviderCommand
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
            // Отправляем команду на создание пользователя по данным внешнего логина
            user = await mediator.Send(new CreateUserExternalCommand
            {
                // Данные от внешнего провайдера
                LoginInfo = info
            });
        }

        // Отчищаем куки данных от внешнего провайдера
        await HttpContext.SignOutAsync(info.AuthenticationProperties);

        // Выполняем вход пользователя через внешнюю аутентификацию.
        await signInManager.SignInAsync(user, true);

        // Перенаправляем по url возврата
        return Redirect(returnUrl);
    }
}