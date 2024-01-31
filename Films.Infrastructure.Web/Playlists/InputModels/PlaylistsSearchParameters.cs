namespace Films.Infrastructure.Web.Contracts.Playlists;

public class PlaylistsSearchParameters
{
    public string? Query { get; init; }
    public string? Genre { get; init; }
    public int Page { get; init; } = 1;
}