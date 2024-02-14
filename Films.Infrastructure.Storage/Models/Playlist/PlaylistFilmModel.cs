using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Storage.Models.Playlist;

[PrimaryKey(nameof(FilmId), nameof(PlaylistId))]
public class PlaylistFilmModel
{
    public Guid FilmId { get; set; }
    public Guid PlaylistId { get; set; }

    public PlaylistModel Playlist { get; set; } = null!;
    public Film.FilmModel Film { get; set; } = null!;
}