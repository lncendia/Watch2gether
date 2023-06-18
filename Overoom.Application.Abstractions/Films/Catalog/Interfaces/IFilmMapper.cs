using Overoom.Application.Abstractions.Films.Catalog.DTOs;

namespace Overoom.Application.Abstractions.Films.Catalog.Interfaces;

public interface IFilmMapper
{
    public FilmDto MapFilm(Domain.Films.Entities.Film film);

    public FilmShortDto MapFilmShort(Domain.Films.Entities.Film film);
}