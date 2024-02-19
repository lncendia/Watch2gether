using MediatR;

namespace Films.Application.Abstractions.Commands.Rooms.YoutubeRooms;

/// <summary>
/// Команда на отключение от комнаты ютуб
/// </summary>
public class DisconnectYoutubeRoomCommand : IRequest
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