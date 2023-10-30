using Overoom.Application.Abstractions.PlaylistsManagement.DTOs;

namespace Overoom.Application.Abstractions.PlaylistsManagement.Interfaces;

public interface IPlaylistManagementService
{
    Task LoadAsync(LoadDto playlist);
    Task ChangeAsync(ChangeDto playlist);
    Task DeleteAsync(Guid playlistId);

    Task<PlaylistDto> GetAsync(Guid playlistId);
    Task<List<PlaylistShortDto>> FindAsync(int page, string? query = null);
}