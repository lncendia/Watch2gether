using MediatR;

namespace Films.Application.Abstractions.Commands.Rooms.FilmRooms;

/// <summary>
/// Команда на отключение от комнаты с фильмом
/// </summary>
public class DisconnectRoomCommand : IRequest
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