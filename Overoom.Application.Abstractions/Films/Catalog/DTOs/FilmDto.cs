using Overoom.Domain.Films.Enums;

namespace Overoom.Application.Abstractions.Films.Catalog.DTOs;

public class FilmDto
{
    public FilmDto(Guid id, string name, int year, FilmType type, Uri posterUri,
        string description, double ratingKp, double userRating, IReadOnlyCollection<string> directors,
        IReadOnlyCollection<string> screenWriters, IReadOnlyCollection<string> genres,
        IReadOnlyCollection<string> countries, IReadOnlyCollection<(string name, string desc)> actors,
        int? countSeasons, int? countEpisodes, IReadOnlyCollection<CdnDto> cdn)
    {
        Name = name;
        Year = year;
        Type = type;
        PosterUri = posterUri;
        Description = description;
        UserRating = userRating;
        RatingKp = ratingKp;
        Directors = directors;
        Genres = genres;
        Countries = countries;
        Actors = actors;
        CountSeasons = countSeasons;
        CountEpisodes = countEpisodes;
        CdnList = cdn;
        ScreenWriters = screenWriters;
        Id = id;
    }

    public Guid Id { get; }
    public string Description { get; }
    public FilmType Type { get; }
    public string Name { get; }
    public Uri PosterUri { get; }
    public int Year { get; }
    public double RatingKp { get; }
    public double UserRating { get; }
    public IReadOnlyCollection<CdnDto> CdnList { get; }
    public int? CountSeasons { get; }
    public int? CountEpisodes { get; }

    public IReadOnlyCollection<string> Genres { get; }
    public IReadOnlyCollection<string> Countries { get; }
    public IReadOnlyCollection<string> Directors { get; }
    public IReadOnlyCollection<string> ScreenWriters { get; }
    public IReadOnlyCollection<(string name, string desc)> Actors { get; }
}