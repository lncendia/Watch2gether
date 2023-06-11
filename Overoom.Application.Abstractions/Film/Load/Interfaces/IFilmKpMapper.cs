using Overoom.Application.Abstractions.Film.Kinopoisk.DTOs;
using Overoom.Application.Abstractions.Film.Load.DTOs;

namespace Overoom.Application.Abstractions.Film.Load.Interfaces;

public interface IFilmKpMapper
{
    FilmDto Map(Kinopoisk.DTOs.Film film, FilmStaff staff, IReadOnlyCollection<Season>? seasons);
}