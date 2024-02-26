using MediatR;

namespace Films.Application.Abstractions.Commands.Rooms.YoutubeRooms;

/// <summary>
/// Команда на удаление комнаты ютуб
/// </summary>
public class DeleteRoomCommand : IRequest
{
    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public required Guid RoomId { get; init; }
}