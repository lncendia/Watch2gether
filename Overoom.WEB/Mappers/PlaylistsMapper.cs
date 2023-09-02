using Overoom.Application.Abstractions.Playlists.DTOs;
using Overoom.WEB.Mappers.Abstractions;
using Overoom.WEB.Models.Playlists;

namespace Overoom.WEB.Mappers;

public class PlaylistsMapper : IPlaylistsMapper
{
    
    public PlaylistViewModel Map(PlaylistDto playlist) => new(playlist.Id, playlist.Name, playlist.Genres,
        playlist.Description, playlist.PosterUri, playlist.Updated);
    
}