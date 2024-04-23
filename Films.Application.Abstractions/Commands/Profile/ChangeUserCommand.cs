using MediatR;

namespace Films.Application.Abstractions.Commands.Profile;

/// <summary>
/// Команда для добавления пользователя.
/// </summary>
public class ChangeUserCommand : IRequest
{
    /// <summary>
    /// Уникальный идентификатор пользователя.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public required string UserName { get; init; }

    /// <summary>
    /// URL фотографии пользователя.
    /// </summary>
    public Uri? PhotoUrl { get; init; }
}