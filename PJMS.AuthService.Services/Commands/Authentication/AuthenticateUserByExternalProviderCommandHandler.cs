using MediatR;
using Microsoft.AspNetCore.Identity;
using PJMS.AuthService.Abstractions.Commands.Authentication;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Exceptions;

namespace PJMS.AuthService.Services.Commands.Authentication;

/// <summary>
/// Класс обработчика команды аутентификации пользователя через внешний провайдер.
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
public class AuthenticateUserByExternalProviderCommandHandler(UserManager<AppUser> userManager)
    : IRequestHandler<AuthenticateUserByExternalProviderCommand, AppUser>
{
    /// <summary>
    /// Обработка команды аутентификации пользователя через внешний провайдер.
    /// </summary>
    /// <param name="request">Запрос на аутентификацию пользователя через внешний провайдер.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Объект пользователя в случае успешной аутентификации.</returns>
    /// <exception cref="UserNotFoundException">Вызывается, если пользователь не найден.</exception>
    /// <exception cref="UserLockoutException">Вызывается, если пользователь заблокирован.</exception>
    public async Task<AppUser> Handle(AuthenticateUserByExternalProviderCommand request,
        CancellationToken cancellationToken)
    {
        // Получаем пользователя по данным внешнего провайдера
        var user = await userManager.FindByLoginAsync(request.LoginProvider, request.ProviderKey);

        // Если пользователь не найден, вызываем исключение UserNotFoundException.
        if (user == null) throw new UserNotFoundException();

        // Проверяем заблокирован ли пользователь 
        if (await userManager.IsLockedOutAsync(user))
        {
            // Если пользователь заблокирован, вызываем исключение UserLockoutException.
            throw new UserLockoutException();
        }

        // Cбрасываем счетчик неудачных попыток входа
        await userManager.ResetAccessFailedCountAsync(user);
            
        // Проверяем, включена ли 2фа у пользователя
        var is2FaEnabled = await userManager.GetTwoFactorEnabledAsync(user);

        // Если у пользователя включена 2фа выбрасываем исключение
        if (is2FaEnabled) throw new TwoFactorRequiredException(user);
            
        // Устанавливаем время последнего входа
        user.TimeLastAuth = DateTime.UtcNow;

        // Обновляем данные
        await userManager.UpdateAsync(user);
            
        //Возвращаем пользователя
        return user;
    }
}