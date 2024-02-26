using MediatR;

namespace Films.Application.Abstractions.Commands.Rooms.YoutubeRooms;

/// <summary>
/// Команда на блокировку пользователя в комнате ютуб
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