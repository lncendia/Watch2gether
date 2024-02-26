using MediatR;
using Microsoft.AspNetCore.Identity;
using PJMS.AuthService.Abstractions.Commands.Authentication;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;
using PJMS.AuthService.Abstractions.Exceptions;

namespace PJMS.AuthService.Services.Commands.Authentication;

/// <summary>
/// Обработчик команды прохождения 2FA
/// </summary>
/// <param name="userManager">Менеджер пользователей, предоставленный ASP.NET Core Identity.</param>
public class AuthenticateTwoFactorCommandHandler(UserManager<AppUser> userManager)
    : IRequestHandler<AuthenticateTwoFactorCommand, AppUser>
{
    /// <summary>
    /// Название EmailTokenProvider'а
    /// </summary>
    private const string EmailTokenProvider = "Email";

    /// <summary>
    /// Метод обработки команды прохождения 2FA
    /// </summary>
    /// <param name="request">Запрос на прохождение 2FA</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Возвращает аутентифицированного пользователя</returns>
    /// <exception cref="UserNotFoundException">Вызывается, если пользователь не был найден</exception>
    /// <exception cref="InvalidCodeException">Вызывается, если код аутентификации не верен</exception>
    /// <exception cref="ArgumentOutOfRangeException">Возникает, при неопознанном типе кода сброса 2фа</exception>
    public async Task<AppUser> Handle(AuthenticateTwoFactorCommand request, CancellationToken cancellationToken)
    {
        // Поиск пользователя по идентификатору
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        // Вызываем исключение UserNotFoundException если не найден пользователь
        if (user == null) throw new UserNotFoundException();

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

        // Если код неверный, выбрасываем исключение
        if (!result) throw new InvalidCodeException();

        // Устанавливаем время последнего входа
        user.TimeLastAuth = DateTime.UtcNow;

        // Обновляем данные
        await userManager.UpdateAsync(user);

        // Возвращаем аутентифицированного пользователя
        return user;
    }
}