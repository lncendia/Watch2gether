using MediatR;

namespace PJMS.AuthService.Abstractions.Commands.Email;

/// <summary>
/// Команда для запроса изменения электронной почты.
/// </summary>
public class RequestChangeEmailCommand : IRequest
{
    /// <summary>
    /// Получает или задает идентификатор пользователя.
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Получает или задает новую электронную почту пользователя.
    /// </summary>
    public required string NewEmail { get; init; }

    /// <summary>
    /// Получает или задает URL для сброса изменения электронной почты.
    /// </summary>
    public required string ResetUrl { get; init; }
}