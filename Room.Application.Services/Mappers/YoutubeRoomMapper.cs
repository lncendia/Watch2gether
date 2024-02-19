using Room.Application.Abstractions.Queries.DTOs.YoutubeRoom;
using Room.Domain.YoutubeRooms;
using Room.Domain.YoutubeRooms.Entities;

namespace Room.Application.Services.Mappers;

/// <summary>
/// Статический класс для преобразования комнаты в DTO
/// </summary>
public static class YoutubeRoomMapper
{
    /// <summary>
    /// Преобразует комнату в DTO
    /// </summary>
    /// <param name="room">Комната</param>
    /// <returns>DTO</returns>
    public static YoutubeRoomDto Map(YoutubeRoom room)
    {
        // Создаем DTO и возвращаем
        return new YoutubeRoomDto
        {
            OwnerId = room.Owner.Id,
            Messages = room.Messages,
            Viewers = room.Viewers.Select(Map).ToArray(),
            Videos = room.Videos,
            VideoAccess = room.VideoAccess,
            Id = room.Id
        };
    }

    /// <summary>
    /// Преобразует зрителя в DTO
    /// </summary>
    /// <param name="viewer">Зритель</param>
    /// <returns>DTO</returns>
    private static YoutubeViewerDto Map(YoutubeViewer viewer)
    {
        // Создаем DTO для зрителя и возвращаем
        return new YoutubeViewerDto
        {
            Id = viewer.Id,
            Nickname = viewer.Nickname,
            PhotoUrl = viewer.PhotoUrl,
            Pause = viewer.Pause,
            FullScreen = viewer.FullScreen,
            TimeLine = viewer.TimeLine,
            Allows = viewer.Allows,
            Online = viewer.Online,
            VideoId = viewer.VideoId
        };
    }
}