using MediatR;
using PJMS.AuthService.Abstractions.Entities;

namespace PJMS.AuthService.Abstractions.Commands.Profile;

/// <summary>
/// Команда для изменения аватара пользователя.
/// </summary>
public class ChangeAvatarCommand : IRequest<AppUser>
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Поток данных с аватаром.
    /// </summary>
    public required Stream Thumbnail { get; init; }
}