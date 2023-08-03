using Overoom.Application.Abstractions.FilmsManagement.DTOs;
using Overoom.WEB.Contracts.FilmManagement.Change;
using Overoom.WEB.Contracts.FilmManagement.Load;
using Overoom.WEB.Models.FilmManagement;

namespace Overoom.WEB.Mappers.Abstractions;

public interface IFilmManagementMapper
{
    ChangeParameters Map(FilmDto dto);
    ChangeDto Map(ChangeParameters parameters);
    LoadDto Map(LoadParameters parameters);
    FilmViewModel Map(FilmShortDto dto);
    FilmInfoViewModel Map(Application.Abstractions.FilmsInformation.DTOs.FilmDto dto);
}