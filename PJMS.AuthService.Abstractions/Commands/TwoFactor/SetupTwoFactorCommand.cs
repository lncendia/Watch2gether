using MediatR;
using PJMS.AuthService.Abstractions.Entities;

namespace PJMS.AuthService.Abstractions.Commands.TwoFactor;

/// <summary>
/// Команда для получения аутентификатора для подключения 2FA
/// </summary>
public class SetupTwoFactorCommand : IRequest<(AppUser user, string token)>
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public required Guid UserId { get; init; }
}