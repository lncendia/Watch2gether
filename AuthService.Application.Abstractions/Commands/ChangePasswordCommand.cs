using AuthService.Application.Abstractions.Entities;
using MediatR;

namespace AuthService.Application.Abstractions.Commands;

/// <summary>
/// Команда для изменения пароля пользователя.
/// </summary>
public class ChangePasswordCommand : IRequest<UserData>
{
    /// <summary>
    /// Инициализирует новый экземпляр класса ChangePasswordCommand.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="oldPassword">Старый пароль пользователя.</param>
    /// <param name="newPassword">Новый пароль пользователя.</param>
    /// <exception cref="ArgumentException">Вызывается, если новый пароль совпадает со старым паролем.</exception>
    public ChangePasswordCommand(long id, string? oldPassword, string newPassword)
    {
        if (oldPassword == newPassword)
            throw new ArgumentException("The new password should be different from the current one.",
                nameof(newPassword));
        Id = id;
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }

    /// <summary>
    /// Получает идентификатор пользователя.
    /// </summary>
    public long Id { get; }

    /// <summary>
    /// Получает старый пароль пользователя.
    /// </summary>
    public string? OldPassword { get; }

    /// <summary>
    /// Получает новый пароль пользователя.
    /// </summary>
    public string NewPassword { get; }
}