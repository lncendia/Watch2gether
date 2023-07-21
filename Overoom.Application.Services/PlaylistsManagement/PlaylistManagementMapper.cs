using Overoom.Application.Abstractions.PlaylistsManagement.DTOs;
using Overoom.Application.Abstractions.PlaylistsManagement.Interfaces;
using Overoom.Domain.Playlists.Entities;

namespace Overoom.Application.Services.PlaylistsManagement;

public class PlaylistManagementMapper : IPlaylistManagementMapper
{
    public PlaylistDto MapGet(Playlist playlist)
    {
        throw new NotImplementedException();
    }

    public PlaylistShortDto MapShort(Playlist playlist)
    {
        throw new NotImplementedException();
    }
}