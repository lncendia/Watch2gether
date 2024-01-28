using AuthService.Application.Abstractions.Entities;
using MediatR;

namespace AuthService.Application.Abstractions.Commands;

/// <summary>
/// Команда для создания пользователя.
/// </summary>
public class CreateUserCommand : IRequest<UserData>
{
    /// <summary>
    /// Получает или задает пароль пользователя.
    /// </summary>
    public required string Password { get; init; }

    /// <summary>
    /// Получает или задает электронную почту пользователя.
    /// </summary>
    public required string Email { get; init; }
    
    /// <summary>
    /// Получает или задает имя пользователя.
    /// </summary>
    public required string Username { get; init; }

    /// <summary>
    /// Получает или задает URL для подтверждения пользователя.
    /// </summary>
    public required string ConfirmUrl { get; init; }
}