using Overoom.Application.Abstractions.Films.Load.DTOs;
using Overoom.WEB.Contracts.FilmLoad;
using Overoom.WEB.Models.FilmLoad;

namespace Overoom.WEB.Mappers.Abstractions;

public interface IFilmLoadMapper
{
    FilmLoadDto Map(FilmLoadParameters parameters);
    FilmViewModel Map(FilmDto dto);
}