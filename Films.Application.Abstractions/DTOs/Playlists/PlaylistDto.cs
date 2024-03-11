namespace Films.Application.Abstractions.DTOs.Playlists;

public class PlaylistDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required IReadOnlyCollection<string> Genres { get; init; }
    public required string Description { get; init; }
    public required Uri PosterUrl { get; init; }
    public required DateTime Updated { get; init; }
}