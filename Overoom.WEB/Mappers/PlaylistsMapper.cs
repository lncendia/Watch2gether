using Overoom.Application.Abstractions.Playlists.DTOs;
using Overoom.WEB.Contracts.Playlists;
using Overoom.WEB.Mappers.Abstractions;
using Overoom.WEB.Models.Playlists;

namespace Overoom.WEB.Mappers;

public class PlaylistsMapper : IPlaylistsMapper
{
    public PlaylistsSearchParameters Map(SearchParameters model) =>
        new()
        {
            Genre = model.Genre, Query = model.Title
        };

    public PlaylistSearchQuery Map(PlaylistsSearchParameters model) => new(model.Query, model.Genre, model.Page);

    public PlaylistViewModel Map(PlaylistDto playlist) => new(playlist.Id, playlist.Name, playlist.Genres,
        playlist.Description, playlist.PosterUri, playlist.Updated);
    
}