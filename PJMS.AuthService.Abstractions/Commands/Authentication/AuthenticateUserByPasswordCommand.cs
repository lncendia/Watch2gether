using MediatR;
using PJMS.AuthService.Abstractions.Entities;

namespace PJMS.AuthService.Abstractions.Commands.Authentication;

/// <summary>
/// Команда для аутентификации пользователя по паролю.
/// </summary>
public class AuthenticateUserByPasswordCommand : IRequest<AppUser>
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