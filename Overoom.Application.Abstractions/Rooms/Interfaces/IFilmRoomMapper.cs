using Overoom.Application.Abstractions.Rooms.DTOs.Film;
using Overoom.Domain.Rooms.FilmRoom.Entities;

namespace Overoom.Application.Abstractions.Rooms.Interfaces;

public interface IFilmRoomMapper
{
    FilmRoomDto Map(FilmRoom room, Domain.Films.Entities.Film film);
    FilmViewerDto Map(FilmViewer v);
}