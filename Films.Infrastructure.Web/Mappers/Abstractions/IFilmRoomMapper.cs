using Films.Application.Abstractions.Rooms.DTOs.Film;
using Films.Infrastructure.Web.Models.Rooms.FilmRoom;

namespace Films.Infrastructure.Web.Mappers.Abstractions;

public interface IFilmRoomMapper
{
    FilmRoomViewModel Map(FilmRoomDto dto, int id, string url);
}