using Overoom.Application.Abstractions.Playlists.DTOs;
using Overoom.Domain.Playlists.Entities;

namespace Overoom.Application.Abstractions.Playlists.Interfaces;

public interface IPlaylistsMapper
{
    PlaylistDto Map(Playlist playlist);
}