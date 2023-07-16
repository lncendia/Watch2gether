using Overoom.Application.Abstractions.FilmsManagement.DTOs;
using Overoom.Application.Abstractions.Kinopoisk.DTOs;

namespace Overoom.Application.Abstractions.FilmsManagement.Interfaces;

public interface IFilmKpMapper
{
    FilmDto Map(Film film, FilmStaff staff, IReadOnlyCollection<Season>? seasons);
}