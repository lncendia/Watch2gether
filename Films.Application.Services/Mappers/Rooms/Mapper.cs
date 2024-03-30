using Films.Application.Abstractions.DTOs.Rooms;
using Films.Domain.Films;
using Films.Domain.Rooms.FilmRooms;
using Films.Domain.Rooms.YoutubeRooms;

namespace Films.Application.Services.Mappers.Rooms;

internal class Mapper
{
    internal static FilmRoomShortDto Map(FilmRoom room, Film film) => new()
    {
        Title = film.Title,
        PosterUrl = film.PosterUrl,
        Year = film.Year,
        UserRating = film.UserRating,
        Description = film.ShortDescription,
        RatingKp = film.RatingKp,
        RatingImdb = film.RatingImdb,
        IsSerial = film.IsSerial,
        Id = room.Id,
        ViewersCount = room.Viewers.Count,
        FilmId = film.Id,
        IsPrivate = !string.IsNullOrEmpty(room.Code)
    };

    internal static YoutubeRoomShortDto Map(YoutubeRoom room) => new()
    {
        Id = room.Id,
        ViewersCount = room.Viewers.Count,
        VideoAccess = room.VideoAccess,
        IsPrivate = !string.IsNullOrEmpty(room.Code)
    };
}