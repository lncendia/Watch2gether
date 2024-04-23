using MediatR;
using PJMS.AuthService.Abstractions.Entities;

namespace PJMS.AuthService.Abstractions.Commands.Email;

/// <summary>
/// Команда для изменения электронной почты пользователя.
/// </summary>
public class ChangeEmailCommand : IRequest<AppUser>
{
    /// <summary>
    /// Получает или задает идентификатор пользователя.
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Получает или задает код подтверждения.
    /// </summary>
    public required string Code { get; init; }

    /// <summary>
    /// Получает или задает новую электронную почту пользователя.
    /// </summary>
    public required string NewEmail { get; init; }
}