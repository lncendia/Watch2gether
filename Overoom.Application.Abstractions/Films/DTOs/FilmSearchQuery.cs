using Overoom.Domain.Films.Enums;

namespace Overoom.Application.Abstractions.Films.DTOs;

public class FilmSearchQuery
{
    public FilmSearchQuery(string? query, string? genre, string? country, string? person,
        FilmType? type, Guid? playlist, int page)
    {
        Query = query;
        Genre = genre;
        Country = country;
        Type = type;
        Page = page;
        Playlist = playlist;
        Person = person;
    }

    public string? Query { get; }
    public string? Genre { get; }
    public string? Country { get; }
    public string? Person { get; }
    public FilmType? Type { get; }
    public Guid? Playlist { get; }
    public int Page { get; }
}