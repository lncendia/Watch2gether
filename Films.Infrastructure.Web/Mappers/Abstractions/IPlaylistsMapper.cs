using Films.Application.Abstractions.Playlists.DTOs;
using Films.Application.Abstractions.Queries.Playlists.DTOs;
using Films.Infrastructure.Web.Contracts.Playlists;
using Films.Infrastructure.Web.Models.Playlists;

namespace Films.Infrastructure.Web.Mappers.Abstractions;

public interface IPlaylistsMapper
{
    public PlaylistsSearchParameters Map(SearchParameters model);
    public PlaylistSearchQuery Map(PlaylistsSearchParameters model);
    public PlaylistViewModel Map(PlaylistDto playlist);
}