using Films.Infrastructure.Storage.Models.Films;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Storage.Models.Playlists;

[PrimaryKey(nameof(FilmId), nameof(PlaylistId))]
public class PlaylistFilmModel
{
    public Guid FilmId { get; set; }
    public Guid PlaylistId { get; set; }

    public PlaylistModel Playlist { get; set; } = null!;
    public FilmModel Film { get; set; } = null!;
}