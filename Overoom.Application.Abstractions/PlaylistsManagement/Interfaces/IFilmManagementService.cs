using Overoom.Application.Abstractions.PlaylistsManagement.DTOs;

namespace Overoom.Application.Abstractions.PlaylistsManagement.Interfaces;

public interface IPlaylistManagementService
{
    Task LoadAsync(LoadDto film);
    Task ChangeAsync(ChangeDto film);
    Task DeleteAsync(Guid filmId);

    Task<PlaylistDto> GetAsync(Guid filmId);
    Task<List<PlaylistShortDto>> FindAsync(int page, string? query = null);
}