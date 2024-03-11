using Films.Application.Abstractions.DTOs.Rooms;
using Films.Domain.Films;
using Films.Domain.Rooms.FilmRooms;
using Films.Domain.Rooms.YoutubeRooms;
using Films.Domain.Servers;

namespace Films.Application.Services.Mappers.Rooms;

internal class Mapper
{
    internal static FilmRoomDto Map(FilmRoom room, Film film, Server server) => new()
    {
        Title = film.Title,
        PosterUrl = film.PosterUrl,
        Year = film.Year,
        UserRating = film.UserRating,
        Description = film.ShortDescription,
        IsSerial = film.IsSerial,
        Id = room.Id,
        ViewersCount = room.Viewers.Count,
        ServerUrl = server.Url,
        IsClosed = !string.IsNullOrEmpty(room.Code)
    };

    internal static YoutubeRoomDto Map(YoutubeRoom room, Server server) => new()
    {
        Id = room.Id,
        ViewersCount = room.Viewers.Count,
        ServerUrl = server.Url,
        VideoAccess = room.VideoAccess,
        IsClosed = !string.IsNullOrEmpty(room.Code)
    };
}