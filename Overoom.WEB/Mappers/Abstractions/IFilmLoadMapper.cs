using Overoom.Application.Abstractions.FilmsManagement.DTOs;
using Overoom.WEB.Contracts.FilmLoad;
using Overoom.WEB.Models.FilmLoad;

namespace Overoom.WEB.Mappers.Abstractions;

public interface IFilmLoadMapper
{
    LoadDto Map(LoadParameters parameters);
    FilmViewModel Map(FilmDto dto);
}