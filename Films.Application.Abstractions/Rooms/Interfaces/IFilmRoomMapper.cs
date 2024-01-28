using Films.Application.Abstractions.Rooms.DTOs.Film;
using Films.Domain.Films.Entities;

namespace Films.Application.Abstractions.Rooms.Interfaces;

public interface IFilmRoomMapper
{
    FilmRoomDto Map(FilmRoom room, Film film);
    FilmViewerDto Map(FilmViewer v);
}