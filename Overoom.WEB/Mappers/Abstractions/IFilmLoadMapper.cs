using Overoom.Application.Abstractions.FilmsInformation.DTOs;
using Overoom.Application.Abstractions.FilmsManagement.DTOs;
using Overoom.WEB.Contracts.FilmLoad;
using Overoom.WEB.Models.FilmLoad;
using FilmDto = Overoom.Application.Abstractions.FilmsInformation.DTOs.FilmDto;

namespace Overoom.WEB.Mappers.Abstractions;

public interface IFilmLoadMapper
{
    LoadDto Map(LoadParameters parameters);
    FilmViewModel Map(FilmDto dto);
}