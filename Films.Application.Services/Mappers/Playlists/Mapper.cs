using Films.Application.Abstractions.DTOs.Films;
using Films.Application.Abstractions.DTOs.Playlists;
using Films.Domain.Films;
using Films.Domain.Playlists;

namespace Films.Application.Services.Mappers.Playlists;

internal class Mapper
{
    internal static PlaylistDto Map(Playlist playlist) => new()
    {
        Id = playlist.Id,
        Name = playlist.Name,
        Genres = playlist.Genres,
        Description = playlist.Description,
        PosterUrl = playlist.PosterUrl,
        Updated = playlist.Updated
    };
}