using MediatR;
using Microsoft.AspNetCore.Identity;
using PJMS.AuthService.Abstractions.Commands.TwoFactor;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Exceptions;

namespace PJMS.AuthService.Services.Commands.TwoFactor;

/// <summary>
/// Обработчик команды верификации подключения пользователю 2FA
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
public class VerifySetupTwoFactorTokenCommandHandler(UserManager<AppUser> userManager)
    : IRequestHandler<VerifySetupTwoFactorTokenCommand, IReadOnlyCollection<string>>
{
    /// <summary>
    /// Количество кодов восстановления для генерации
    /// </summary>
    private const int RecoveryCodesCount = 5;

    /// <summary>
    /// Метод верификации и подключения 2FA пользователю
    /// </summary>
    /// <param name="request">Запрос на подключение 2FA</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Возвращает резервные коды аутентификации</returns>
    /// <exception cref="UserNotFoundException">Вызывается, если пользователь не был найден</exception>
    /// <exception cref="InvalidCodeException">Вызывается, если предоставленный код не был верифицирован</exception>
    public async Task<IReadOnlyCollection<string>> Handle(VerifySetupTwoFactorTokenCommand request,
        CancellationToken cancellationToken)
    {
        // Ищем пользователя по идентификатору
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        // Вызываем исключение если пользователь не найден
        if (user == null) throw new UserNotFoundException();

        // Вызываем исключение, если 2FA уже подключена
        if (await userManager.GetTwoFactorEnabledAsync(user)) throw new TwoFactorAlreadyEnabledException();
        
        // Проверяем токен аутентификатора пользователя с помощью провайдера токена, подключенного при конфигурации
        var isValid = await userManager.VerifyTwoFactorTokenAsync(user,
            userManager.Options.Tokens.AuthenticatorTokenProvider, request.Code);

        // Если токен не валидный - выбрасываем исключение
        if (!isValid) throw new InvalidCodeException();

        // Подключаем 2FA пользователю
        await userManager.SetTwoFactorEnabledAsync(user, true);

        // Генерируем резервные коды 2fa аутентификации
        var codes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, RecoveryCodesCount);

        // Возвращаем коды
        return codes?.ToArray() ?? [];
    }
}