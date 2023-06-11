using Overoom.Infrastructure.Storage.Models.Film;

namespace Overoom.Infrastructure.Storage.Models.Playlist;

public class PlaylistFilmModel
{
    public long Id { get; set; }
    public Guid FilmId { get; set; }
    public FilmModel Film { get; set; } = null!;
}