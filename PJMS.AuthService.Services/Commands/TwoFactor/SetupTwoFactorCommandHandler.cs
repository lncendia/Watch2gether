using MediatR;
using Microsoft.AspNetCore.Identity;
using PJMS.AuthService.Abstractions.Commands.TwoFactor;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Exceptions;

namespace PJMS.AuthService.Services.Commands.TwoFactor;

/// <summary>
/// Обработчик команды получения аутентификатора пользователем для подключения 2FA
/// </summary>
/// <param name="userManager"></param>
public class SetupTwoFactorCommandHandler(UserManager<AppUser> userManager)
    : IRequestHandler<SetupTwoFactorCommand, (AppUser, string)>
{
    /// <summary>
    /// Метод установки аутентификатора пользователя для подключения 2FA
    /// </summary>
    /// <param name="request">Запрос на установку аутентификатора</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Кортеж, содержащий пользователя и код установленного аутентификатора</returns>
    /// <exception cref="UserNotFoundException">Вызывается, если пользователь не был найден</exception>
    /// <exception cref="TwoFactorAlreadyEnabledException">Вызывается, если 2FA уже подключена, и аутентификатор уже был установлен</exception>
    public async Task<(AppUser, string)> Handle(SetupTwoFactorCommand request, CancellationToken cancellationToken)
    {
        // Поиск пользователя по адресу электронной почты.
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        // Вызываем исключение, если пользователь не найден
        if (user == null) throw new UserNotFoundException();

        // Вызываем исключение, если 2FA уже подключена
        if (await userManager.GetTwoFactorEnabledAsync(user)) throw new TwoFactorAlreadyEnabledException();

        // Устанавливаем аутентификатор и получаем в виде строки
        var authenticatorKey = await userManager.GetAuthenticatorKeyAsync(user);
        
        // Если аутентификатор установлен, возвращаем его
        if (authenticatorKey != null) return (user, authenticatorKey);
        
        // Если нет, сбрасываем аутентификатор
        await userManager.ResetAuthenticatorKeyAsync(user);
            
        // Устанавливаем аутентификатор и получаем в виде строки
        authenticatorKey = await userManager.GetAuthenticatorKeyAsync(user);

        // Возвращаем кортеж из пользователя и аутентификатора
        return (user, authenticatorKey!);
    }
}