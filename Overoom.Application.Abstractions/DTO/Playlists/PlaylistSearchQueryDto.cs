namespace Overoom.Application.Abstractions.DTO.Playlists;

public class PlaylistSearchQueryDto
{
    public PlaylistSearchQueryDto(string? name, SortBy sortBy, int page, bool inverseOrder)
    {
        SortBy = sortBy;
        Page = page;
        InverseOrder = inverseOrder;
    }

    public string? Name { get; }
    public SortBy SortBy { get; }
    public bool InverseOrder { get; }
    public int Page { get; }
}

public enum SortBy
{
    Date,
    CountFilms
}