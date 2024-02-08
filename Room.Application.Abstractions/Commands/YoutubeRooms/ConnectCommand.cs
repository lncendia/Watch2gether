using MediatR;
using Room.Application.Abstractions.DTOs.YoutubeRoom;

namespace Room.Application.Abstractions.Commands.YoutubeRooms;

/// <summary>
/// Команда на подключение к комнате
/// </summary>
public class ConnectCommand : IRequest<YoutubeRoomDto>
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public required Guid RoomId { get; init; }

    /// <summary>
    /// Проверочный код
    /// </summary>
    public string? Code { get; init; }
}