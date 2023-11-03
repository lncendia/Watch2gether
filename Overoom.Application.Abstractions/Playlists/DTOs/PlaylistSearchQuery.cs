namespace Overoom.Application.Abstractions.Playlists.DTOs;

public class PlaylistSearchQuery
{
    public PlaylistSearchQuery(string? query, string? genre, int page)
    {
        Query = query;
        Genre = genre;
        Page = page;
    }

    public string? Query { get; }
    public string? Genre { get; }
    public int Page { get; }
}