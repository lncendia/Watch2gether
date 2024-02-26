using MediatR;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;

namespace PJMS.AuthService.Abstractions.Commands.Create;

/// <summary>
/// Команда для создания пользователя.
/// </summary>
public class CreateUserCommand : IRequest<AppUser>
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
    /// Получает или задает URL для подтверждения пользователя.
    /// </summary>
    public required string ConfirmUrl { get; init; }

    /// <summary>
    /// Получает или задает локализацию пользователя.
    /// </summary>
    public required Localization Locale { get; init; }
}