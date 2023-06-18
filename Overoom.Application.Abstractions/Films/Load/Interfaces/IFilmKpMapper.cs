using Overoom.Application.Abstractions.Films.Kinopoisk.DTOs;
using Overoom.Application.Abstractions.Films.Load.DTOs;

namespace Overoom.Application.Abstractions.Films.Load.Interfaces;

public interface IFilmKpMapper
{
    FilmDto Map(Kinopoisk.DTOs.Film film, FilmStaff staff, IReadOnlyCollection<Season>? seasons);
}