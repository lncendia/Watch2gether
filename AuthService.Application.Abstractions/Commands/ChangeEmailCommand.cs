using AuthService.Application.Abstractions.Entities;
using MediatR;

namespace AuthService.Application.Abstractions.Commands;

/// <summary>
/// Команда для изменения электронной почты пользователя.
/// </summary>
public class ChangeEmailCommand : IRequest<UserData>
{
    /// <summary>
    /// Получает или задает идентификатор пользователя.
    /// </summary>
    public required long Id { get; init; }

    /// <summary>
    /// Получает или задает код подтверждения.
    /// </summary>
    public required string Code { get; init; }

    /// <summary>
    /// Получает или задает новую электронную почту пользователя.
    /// </summary>
    public required string NewEmail { get; init; }
}