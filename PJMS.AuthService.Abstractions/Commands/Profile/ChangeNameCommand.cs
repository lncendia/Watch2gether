using MediatR;
using PJMS.AuthService.Abstractions.Entities;

namespace PJMS.AuthService.Abstractions.Commands.Profile;

/// <summary>
/// Команда для изменения имени пользователя.
/// </summary>
public class ChangeNameCommand : IRequest<AppUser>
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Новое имя пользователя.
    /// </summary>
    public required string Name { get; init; }
}