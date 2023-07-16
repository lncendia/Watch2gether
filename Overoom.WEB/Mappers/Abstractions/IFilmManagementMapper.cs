using Overoom.Application.Abstractions.FilmsManagement.DTOs;
using Overoom.WEB.Contracts.FilmManagement;
using Overoom.WEB.Models.FilmManagement;

namespace Overoom.WEB.Mappers.Abstractions;

public interface IFilmManagementMapper
{
    ChangeParameters Map(GetDto dto);
    ChangeDto Map(ChangeParameters parameters);
    FilmViewModel Map(FilmShortDto dto);
}