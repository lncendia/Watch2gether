using MediatR;

namespace PJMS.AuthService.Abstractions.Commands.Email;

/// <summary>
/// Команда для проверки электронной почты пользователя.
/// </summary>
public class VerifyEmailCommand : IRequest
{
    /// <summary>
    /// Получает или задает идентификатор пользователя.
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Получает или задает код проверки.
    /// </summary>
    public required string Code { get; init; }
}