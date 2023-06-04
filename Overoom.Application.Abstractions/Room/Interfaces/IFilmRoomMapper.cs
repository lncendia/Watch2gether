using Overoom.Application.Abstractions.Room.DTOs.Film;
using Overoom.Domain.Room.FilmRoom.Entities;

namespace Overoom.Application.Abstractions.Room.Interfaces;

public interface IFilmRoomMapper
{
    FilmRoomDto Map(FilmRoom room, Domain.Film.Entities.Film film);
    FilmViewerDto Map(FilmViewer v);
}