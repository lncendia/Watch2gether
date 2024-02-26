using MediatR;

namespace Films.Application.Abstractions.Commands.Rooms.FilmRooms;

/// <summary>
/// Команда на блокировку пользователя в комнате с фильмом
/// </summary>
public class BlockViewerCommand : IRequest
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public required Guid RoomId { get; init; }
}