using MediatR;
using Room.Application.Abstractions.Commands.Rooms;
using Room.Application.Abstractions.DTOs.FilmRooms;

namespace Room.Application.Abstractions.Commands.FilmRooms;

/// <summary>
/// Команда на подключение к комнате
/// </summary>
public class ConnectCommand : IRequest<FilmRoomDto>
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