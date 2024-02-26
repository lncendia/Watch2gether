using MediatR;
using PJMS.AuthService.Abstractions.Entities;

namespace PJMS.AuthService.Abstractions.Commands.Password;

/// <summary>
/// Команда для изменения пароля пользователя.
/// </summary>
public class ChangePasswordCommand : IRequest<AppUser>
{
    /// <summary>
    /// Инициализирует новый экземпляр класса ChangePasswordCommand.
    /// </summary>
    /// <param name="oldPassword">Старый пароль пользователя.</param>
    /// <param name="newPassword">Новый пароль пользователя.</param>
    /// <exception cref="ArgumentException">Вызывается, если новый пароль совпадает со старым паролем.</exception>
    public ChangePasswordCommand(string? oldPassword, string newPassword)
    {
        if (oldPassword == newPassword)
            throw new ArgumentException("The new password should be different from the current one.",
                nameof(newPassword));
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }

    /// <summary>
    /// Получает идентификатор пользователя.
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Получает старый пароль пользователя.
    /// </summary>
    public string? OldPassword { get; }

    /// <summary>
    /// Получает новый пароль пользователя.
    /// </summary>
    public string NewPassword { get; }
}