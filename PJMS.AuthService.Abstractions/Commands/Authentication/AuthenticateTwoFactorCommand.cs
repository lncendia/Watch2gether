using MediatR;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;

namespace PJMS.AuthService.Abstractions.Commands.Authentication;

/// <summary>
/// Команда для прохождения пользователем 2FA
/// </summary>
public class AuthenticateTwoFactorCommand : IRequest<AppUser>
{
    /// <summary>
    /// Получает или задает код для прохождения 2FA
    /// </summary>
    public required string Code { get; init; }

    /// <summary>
    /// Получает или задает идентификатор пользователя.
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Получает или задает тип кода для прохождения 2FA
    /// </summary>
    public required CodeType Type { get; init; }
}