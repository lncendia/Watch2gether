using Overoom.Application.Abstractions.Playlists.DTOs;
using Overoom.WEB.Models.Playlists;

namespace Overoom.WEB.Mappers.Abstractions;

public interface IPlaylistsMapper
{
    public PlaylistViewModel Map(PlaylistDto playlist);
}