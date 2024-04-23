using MediatR;

namespace PJMS.AuthService.Abstractions.Commands.TwoFactor;
/// <summary>
/// Команда для отправки кода 2FA на почту.
/// </summary>
public class RequestTwoFactorCodeEmailCommand : IRequest
{
    /// <summary>
    /// Получает или задает идентификатор пользователя
    /// </summary>
    public required Guid UserId { get; init; }
}