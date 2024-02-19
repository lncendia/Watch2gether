using MediatR;
using Room.Application.Abstractions.Commands.BaseRooms;
using Room.Application.Abstractions.Queries.DTOs.YoutubeRoom;

namespace Room.Application.Abstractions.Commands.YoutubeRooms;

/// <summary>
/// Команда на подключение к комнате
/// </summary>
public class ConnectCommand : IRequest<YoutubeRoomDto>
{
    /// <summary>
    /// Зритель
    /// </summary>
    public required ViewerData Viewer { get; init; }

    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public required Guid RoomId { get; init; }
}