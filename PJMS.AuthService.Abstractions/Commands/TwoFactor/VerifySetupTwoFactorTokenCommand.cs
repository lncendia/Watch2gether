using MediatR;
using PJMS.AuthService.Abstractions.Entities;

namespace PJMS.AuthService.Abstractions.Commands.TwoFactor;

/// <summary>
/// Команда верификации подключения пользователю 2FA
/// </summary>
public class VerifySetupTwoFactorTokenCommand : IRequest<IReadOnlyCollection<string>>
{
    /// <summary>
    /// Получает или задает идентификатор пользователя
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Получает или задает код верификации 2FA
    /// </summary>
    public required string Code { get; init; }
}