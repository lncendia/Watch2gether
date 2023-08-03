using Overoom.Application.Abstractions.PlaylistsManagement.DTOs;
using Overoom.Application.Abstractions.PlaylistsManagement.Interfaces;
using Overoom.Domain.Playlists.Entities;

namespace Overoom.Application.Services.PlaylistsManagement;

public class PlaylistManagementMapper : IPlaylistManagementMapper
{
    public PlaylistDto MapGet(Playlist playlist) => new(playlist.Id, playlist.Name, playlist.Description,
        playlist.PosterUri, playlist.Films);

    public PlaylistShortDto MapShort(Playlist playlist) => new(playlist.Id, playlist.Name, playlist.PosterUri);
}