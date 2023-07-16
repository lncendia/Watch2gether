using Overoom.Domain.Films.Enums;

namespace Overoom.Application.Abstractions.Content.DTOs;

public class FilmSearchQuery
{
    public FilmSearchQuery(string? query, int? minYear, int? maxYear, string? genre, string? country, string? person,
        FilmType? type, Guid? playlist, FilmSortBy sortBy, int page, bool inverseOrder)
    {
        Query = query;
        MinYear = minYear;
        MaxYear = maxYear;
        Genre = genre;
        Country = country;
        Type = type;
        SortBy = sortBy;
        Page = page;
        InverseOrder = inverseOrder;
        Playlist = playlist;
        Person = person;
    }

    public string? Query { get; }
    public int? MinYear { get; }
    public int? MaxYear { get; }
    public string? Genre { get; }
    public string? Country { get; }
    public string? Person { get; }
    public FilmType? Type { get; }
    public Guid? Playlist { get; }
    public FilmSortBy SortBy { get; }
    public bool InverseOrder { get; }
    public int Page { get; }
}

public enum FilmSortBy
{
    Rating,
    Date
}