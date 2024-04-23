using MediatR;

namespace PJMS.AuthService.Abstractions.Commands.Password;

/// <summary>
/// Команда для восстановления пароля.
/// </summary>
public class RecoverPasswordCommand : IRequest
{
    /// <summary>
    /// Получает или задает электронную почту пользователя.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Получает или задает код для восстановления пароля.
    /// </summary>
    public required string Code { get; init; }

    /// <summary>
    /// Получает или задает новый пароль пользователя.
    /// </summary>
    public required string NewPassword { get; init; }
}