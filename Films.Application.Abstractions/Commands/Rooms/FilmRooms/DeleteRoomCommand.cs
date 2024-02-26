using MediatR;

namespace Films.Application.Abstractions.Commands.Rooms.FilmRooms;

/// <summary>
/// Команда на удаление комнаты с фильмом
/// </summary>
public class DeleteRoomCommand : IRequest
{
    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public required Guid RoomId { get; init; }
}