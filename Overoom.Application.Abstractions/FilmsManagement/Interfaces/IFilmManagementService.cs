using Overoom.Application.Abstractions.FilmsManagement.DTOs;

namespace Overoom.Application.Abstractions.FilmsManagement.Interfaces;

public interface IFilmManagementService
{
    Task LoadAsync(LoadDto film);
    Task ChangeAsync(ChangeDto film);
    Task DeleteAsync(Guid filmId);

    Task<FilmDto> GetAsync(Guid filmId);
    Task<List<FilmShortDto>> FindAsync(int page, string? query = null);
}