namespace Overoom.WEB.Contracts.Playlists;

public class PlaylistsSearchParameters
{
    public string? Query { get; set; }
    public string? Genre { get; set; }
    public int Page { get; set; } = 1;
}