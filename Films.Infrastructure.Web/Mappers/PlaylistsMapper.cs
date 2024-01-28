using Films.Application.Abstractions.Playlists.DTOs;
using Films.Application.Abstractions.Queries.Playlists.DTOs;
using Films.Infrastructure.Web.Contracts.Playlists;
using Films.Infrastructure.Web.Mappers.Abstractions;
using Films.Infrastructure.Web.Models.Playlists;

namespace Films.Infrastructure.Web.Mappers;

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