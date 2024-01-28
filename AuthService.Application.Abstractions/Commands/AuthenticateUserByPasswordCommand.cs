using AuthService.Application.Abstractions.Entities;
using MediatR;

namespace AuthService.Application.Abstractions.Commands;

/// <summary>
/// Команда для аутентификации пользователя по паролю.
/// </summary>
public class AuthenticateUserByPasswordCommand : IRequest<UserData>
{
    /// <summary>
    /// Получает или задает электронную почту пользователя.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Получает или задает пароль пользователя.
    /// </summary>
    public required string Password { get; init; }
}