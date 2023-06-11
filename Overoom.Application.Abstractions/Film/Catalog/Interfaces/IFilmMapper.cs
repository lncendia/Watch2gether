using Overoom.Application.Abstractions.Film.Catalog.DTOs;

namespace Overoom.Application.Abstractions.Film.Catalog.Interfaces;

public interface IFilmMapper
{
    public FilmDto MapFilm(Domain.Film.Entities.Film film);

    public FilmShortDto MapFilmShort(Domain.Film.Entities.Film film);
}