using Overoom.Application.Abstractions.Rooms.DTOs.Film;
using Overoom.WEB.Models.Rooms.FilmRoom;

namespace Overoom.WEB.Mappers.Abstractions;

public interface IFilmRoomMapper
{
    FilmRoomViewModel Map(FilmRoomDto dto, int id, string url);
}