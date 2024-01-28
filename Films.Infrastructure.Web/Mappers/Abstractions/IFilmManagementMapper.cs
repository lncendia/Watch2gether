using Films.Application.Abstractions.FilmsManagement.DTOs;
using Films.Application.Abstractions.Queries.FilmsApi.DTOs;
using Films.Infrastructure.Web.Contracts.FilmManagement;
using Films.Infrastructure.Web.Contracts.FilmManagement.Load;
using Films.Infrastructure.Web.Models.FilmManagement;

namespace Films.Infrastructure.Web.Mappers.Abstractions;

public interface IFilmManagementMapper
{
    ChangeFilmInputModel Map(FilmDto dto);
    ChangeDto Map(ChangeFilmInputModel filmInputModel);
    LoadDto Map(AddFilmInputModel inputModel);
    FilmViewModel Map(FilmShortDto dto);
    FilmInfoViewModel Map(FilmApiDto apiDto);
}