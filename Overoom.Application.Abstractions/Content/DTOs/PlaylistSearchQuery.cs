namespace Overoom.Application.Abstractions.Content.DTOs;

public class PlaylistSearchQuery
{
    public PlaylistSearchQuery(string? name, PlaylistSortBy sortBy, int page, bool inverseOrder)
    {
        SortBy = sortBy;
        Page = page;
        InverseOrder = inverseOrder;
        Name = name;
    }

    public string? Name { get; }
    public PlaylistSortBy SortBy { get; }
    public bool InverseOrder { get; }
    public int Page { get; }
}

public enum PlaylistSortBy
{
    Date,
    CountFilms
}