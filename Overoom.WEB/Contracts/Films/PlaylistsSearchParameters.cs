using Overoom.Application.Abstractions.Films.Playlist.DTOs;

namespace Overoom.WEB.Contracts.Films;

public class PlaylistsSearchParameters
{
    public PlaylistsSearchParameters(string? name, SortBy sortBy, bool inverseOrder, int page)
    {
        Name = name;
        SortBy = sortBy;
        InverseOrder = inverseOrder;
        Page = page;
    }

    public string? Name { get; }
    public SortBy SortBy { get; }
    public bool InverseOrder { get; }
    public int Page { get; }
}