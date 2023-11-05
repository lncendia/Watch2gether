using Overoom.Application.Abstractions.Playlists.DTOs;
using Overoom.WEB.Contracts.Playlists;
using Overoom.WEB.Models.Playlists;

namespace Overoom.WEB.Mappers.Abstractions;

public interface IPlaylistsMapper
{
    public PlaylistsSearchParameters Map(SearchParameters model);
    public PlaylistSearchQuery Map(PlaylistsSearchParameters model);
    public PlaylistViewModel Map(PlaylistDto playlist);
}