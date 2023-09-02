using Overoom.Application.Abstractions.Playlists.DTOs;
using Overoom.Application.Abstractions.Playlists.Interfaces;
using Overoom.Domain.Playlists.Entities;

namespace Overoom.Application.Services.Playlists;

public class PlaylistsMapper : IPlaylistsMapper
{
    public PlaylistDto Map(Playlist playlist) => new(playlist.Id, playlist.PosterUri, playlist.Updated,
        playlist.Name, playlist.Description, playlist.Genres);
}