using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Storage.Models.Playlists;

[PrimaryKey(nameof(FilmId), nameof(PlaylistId))]
public class PlaylistFilmModel
{
    public Guid FilmId { get; set; }
    public Guid PlaylistId { get; set; }
}