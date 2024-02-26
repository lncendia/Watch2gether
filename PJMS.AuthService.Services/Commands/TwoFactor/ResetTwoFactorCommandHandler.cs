using MediatR;
using Microsoft.AspNetCore.Identity;
using PJMS.AuthService.Abstractions.Commands.TwoFactor;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;
using PJMS.AuthService.Abstractions.Exceptions;

namespace PJMS.AuthService.Services.Commands.TwoFactor;

/// <summary>
/// Обработчик команды сброса 2фа
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
public class ResetTwoFactorCommandHandler(UserManager<AppUser> userManager)
    : IRequestHandler<ResetTwoFactorCommand, AppUser>
{
    /// <summary>
    /// Название EmailTokenProvider'а
    /// </summary>
    private const string EmailTokenProvider = "Email";

    /// <summary>
    /// Метод обработки команды сброса 2фа
    /// </summary>
    /// <param name="request">Запрос на сброс 2фа</param>
    ///<param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Возвращает пользователя со сброшенной 2фа</returns>
    /// <exception cref="UserNotFoundException">Возникает, если пользователь не был найден</exception>
    /// <exception cref="ArgumentOutOfRangeException">Возникает, при неопознанном типе кода сброса 2фа</exception>
    /// <exception cref="InvalidCodeException">Возникает, при невалидном коде</exception>
    /// <exception cref="TwoFactorNotEnabledException">Возникает, при попытка отключить 2фа, когда она уже отключена</exception>
    public async Task<AppUser> Handle(ResetTwoFactorCommand request, CancellationToken cancellationToken)
    {
        // Получаем пользователя
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        // Если пользователь не найден - выбрасываем исключение
        if (user == null) throw new UserNotFoundException();

        // Если у пользователя уже отключена 2фа - выбрасываем исключение
        if (!await userManager.GetTwoFactorEnabledAsync(user)) throw new TwoFactorNotEnabledException();

        // Верифицируем токен на основе указанного провайдера
        var result = request.Type switch
        {
            //Если код от аутентификатора - указываем AuthenticatorTokenProvider в качестве провайдера валидации
            CodeType.Authenticator => await userManager.VerifyTwoFactorTokenAsync(user,
                userManager.Options.Tokens.AuthenticatorTokenProvider, request.Code),

            //Если код от провайдера Email - указываем EmailTokenProvider в качестве провайдера валидации
            CodeType.Email => await userManager.VerifyTwoFactorTokenAsync(user, EmailTokenProvider, request.Code),

            // Если пришел код восстановления - проверяем его методом RedeemTwoFactorRecoveryCodeAsync
            CodeType.RecoveryCode => (await userManager.RedeemTwoFactorRecoveryCodeAsync(user, request.Code)).Succeeded,

            // При ином значении CodeType выбрасываем исключение
            _ => throw new ArgumentOutOfRangeException(nameof(request))
        };

        // Если код не удалось верифицировать - выбрасываем исключение
        if (!result) throw new InvalidCodeException();

        // Отключаем 2FA у пользователя
        await userManager.SetTwoFactorEnabledAsync(user, false);

        // Сбрасываем аутентификатор пользователя
        await userManager.ResetAuthenticatorKeyAsync(user);

        // Возвращаем пользователя со сброшенной 2фа
        return user;
    }
}