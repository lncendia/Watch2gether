using Overoom.Application.Abstractions.Films.DTOs;
using Overoom.WEB.Contracts.Films;
using Overoom.WEB.Models.Films;

namespace Overoom.WEB.Mappers.Abstractions;

public interface IFilmsMapper
{
    public FilmsSearchParameters Map(SearchParameters model);
    public FilmSearchQuery Map(FilmsSearchParameters model);
    public FilmViewModel Map(FilmDto film);
    public FilmShortViewModel MapShort(FilmDto film);
}