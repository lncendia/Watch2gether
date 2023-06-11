using Overoom.Infrastructure.Storage.Models.Abstractions;

namespace Overoom.Infrastructure.Storage.Models.Playlist;

public class PlaylistModel : IAggregateModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<PlaylistFilmModel> Films { get; set; } = new();
    public DateTime Updated { get; set; }
    public string PosterUri { get; set; } = null!;
}