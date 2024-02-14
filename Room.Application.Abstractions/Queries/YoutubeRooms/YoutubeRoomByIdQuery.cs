using MediatR;
using Room.Application.Abstractions.DTOs.YoutubeRoom;

namespace Room.Application.Abstractions.Queries.YoutubeRooms;

/// <summary>
/// Запрос на получение комнаты
/// </summary>
public class YoutubeRoomByIdQuery : IRequest<YoutubeRoomDto>
{
    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public required Guid Id { get; init; }
}