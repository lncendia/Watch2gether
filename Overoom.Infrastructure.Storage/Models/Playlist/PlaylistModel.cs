using Overoom.Infrastructure.Storage.Models.Abstractions;

namespace Overoom.Infrastructure.Storage.Models.Playlist;

public class PlaylistModel : IAggregateModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<PlaylistFilmModel> Films { get; set; } = new();
    public List<PlaylistGenreModel> Genres { get; set; } = new();
    public DateTime Updated { get; set; }
    public Uri PosterUri { get; set; } = null!;
}