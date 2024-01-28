using Films.Application.Abstractions.Films.DTOs;
using Films.Application.Abstractions.Queries.Films.Dtos;
using Films.Infrastructure.Web.Contracts.Films;
using Films.Infrastructure.Web.Models.Films;

namespace Films.Infrastructure.Web.Mappers.Abstractions;

public interface IFilmsMapper
{
    public FilmsSearchParameters Map(SearchParameters model);
    public FilmSearchQuery Map(FilmsSearchParameters model);
    public FilmViewModel Map(FilmDto film);
    public FilmShortViewModel MapShort(FilmDto film);
}