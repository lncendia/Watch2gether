using Overoom.Domain.Films.Enums;

namespace Overoom.Application.Abstractions.Films.Load.DTOs;

public class FilmDto
{
    public FilmDto(string name, string description, string? shortDescription, double ratingKp, int year,
        FilmType type, Uri posterUri, IReadOnlyCollection<string> genres,
        IReadOnlyCollection<(string name, string description)> actors,
        IReadOnlyCollection<string> countries, IReadOnlyCollection<string> directors,
        IReadOnlyCollection<string> screenwriters,
        int? countSeasons, int? countEpisodes)
    {
        Name = name;
        Year = year;
        Type = type;
        PosterUri = posterUri;
        CountSeasons = countSeasons;
        CountEpisodes = countEpisodes;
        Description = description;
        RatingKp = ratingKp;
        ShortDescription = shortDescription;
        Genres = genres;
        Actors = actors;
        Countries = countries;
        Screenwriters = screenwriters;
        Directors = directors;
    }

    public string Description { get; }
    public string? ShortDescription { get; }
    public FilmType Type { get; }
    public Uri PosterUri { get; }
    public string Name { get; }
    public int Year { get; }
    public double RatingKp { get; }
    public int? CountSeasons { get; }
    public int? CountEpisodes { get; }

    public IReadOnlyCollection<string> Countries { get; }
    public IReadOnlyCollection<(string name, string description)> Actors { get; }
    public IReadOnlyCollection<string> Directors { get; }
    public IReadOnlyCollection<string> Genres { get; }
    public IReadOnlyCollection<string> Screenwriters { get; }
}