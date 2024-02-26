using MediatR;
using PJMS.AuthService.Abstractions.Enums;

namespace PJMS.AuthService.Abstractions.Commands.Profile;

/// <summary>
/// Команда для изменения локали пользователя.
/// </summary>
public class ChangeLocaleCommand : IRequest
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Локализация пользователя.
    /// </summary>
    public required Localization Localization { get; init; }
}