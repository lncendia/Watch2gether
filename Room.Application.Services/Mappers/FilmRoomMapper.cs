using Room.Application.Abstractions.Queries.DTOs.FilmRoom;
using Room.Domain.FilmRooms;
using Room.Domain.FilmRooms.Entities;

namespace Room.Application.Services.Mappers;

/// <summary>
/// Статический класс для преобразования комнаты в DTO
/// </summary>
public static class FilmRoomMapper
{
    /// <summary>
    /// Преобразует комнату в DTO
    /// </summary>
    /// <param name="room">Комната</param>
    /// <returns>DTO</returns>
    public static FilmRoomDto Map(FilmRoom room)
    {
        // Создаем DTO и возвращаем
        return new FilmRoomDto
        {
            Id = room.Id,
            Title = room.Title,
            CdnUrl = room.CdnUrl,
            IsSerial = room.IsSerial,
            OwnerId = room.Owner.Id,
            Messages = room.Messages,
            Viewers = room.Viewers.Select(Map).ToArray()
        };
    }

    /// <summary>
    /// Преобразует зрителя в DTO
    /// </summary>
    /// <param name="viewer">Зритель</param>
    /// <returns>DTO</returns>
    private static FilmViewerDto Map(FilmViewer viewer)
    {
        // Создаем DTO для зрителя и возвращаем
        return new FilmViewerDto
        {
            Id = viewer.Id,
            Nickname = viewer.Nickname,
            PhotoUrl = viewer.PhotoUrl,
            Pause = viewer.Pause,
            FullScreen = viewer.FullScreen,
            TimeLine = viewer.TimeLine,
            Allows = viewer.Allows,
            Online = viewer.Online,
            Season = viewer.Season,
            Series = viewer.Series
        };
    }
}