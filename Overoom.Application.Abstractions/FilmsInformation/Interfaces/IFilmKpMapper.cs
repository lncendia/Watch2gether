using Overoom.Application.Abstractions.FilmsInformation.DTOs;
using Overoom.Application.Abstractions.Kinopoisk.DTOs;

namespace Overoom.Application.Abstractions.FilmsInformation.Interfaces;

public interface IFilmKpMapper
{
    FilmDto Map(Film film, FilmStaff staff, IReadOnlyCollection<Season>? seasons, IReadOnlyCollection<Cdn> cdn);
}