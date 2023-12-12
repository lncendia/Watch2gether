using System.ComponentModel.DataAnnotations.Schema;

namespace Overoom.Infrastructure.Storage.Models.Playlist;

public class PlaylistFilmModel
{
    public long Id { get; set; }
    public Guid FilmId { get; set; }

    public PlaylistModel Playlist { get; set; } = null!;
    public Film.FilmModel Film { get; set; } = null!;
}