using MediatR;

namespace Films.Application.Abstractions.Commands.YoutubeRooms;

/// <summary>
/// Команда на отключение от комнаты ютуб
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