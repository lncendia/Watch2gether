namespace Films.Infrastructure.Web.Playlists.ViewModels;

public class PlaylistViewModel
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required IEnumerable<string> Genres { get; init; }
    public required string Description { get; init; }
    public required string PosterUrl { get; init; }
    public required DateTime Updated { get; init; }
}