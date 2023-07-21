using Overoom.Application.Abstractions.FilmsManagement.DTOs;
using Overoom.Domain.Films.Enums;

namespace Overoom.Application.Abstractions.FilmsInformation.DTOs;

public class FilmDto
{
    public FilmDto(string name, string? description, string? shortDescription, double rating, int year,
        FilmType type, Uri? posterUri, IReadOnlyCollection<string> genres,
        IReadOnlyCollection<(string name, string? description)> actors,
        IReadOnlyCollection<string> countries, IReadOnlyCollection<string> directors,
        IReadOnlyCollection<string> screenwriters,
        int? countSeasons, int? countEpisodes, IReadOnlyCollection<CdnDto> cdn)
    {
        Name = name;
        Year = year;
        Type = type;
        PosterUri = posterUri;
        CountSeasons = countSeasons;
        CountEpisodes = countEpisodes;
        Cdn = cdn;
        Description = description;
        Rating = rating;
        ShortDescription = shortDescription;
        Genres = genres;
        Actors = actors;
        Countries = countries;
        Screenwriters = screenwriters;
        Directors = directors;
    }

    public string? Description { get; }
    public string? ShortDescription { get; }
    public FilmType Type { get; }
    public Uri? PosterUri { get; }
    public string Name { get; }
    public int Year { get; }
    public double Rating { get; }
    public int? CountSeasons { get; }
    public int? CountEpisodes { get; }

    public IReadOnlyCollection<CdnDto> Cdn { get; }
    public IReadOnlyCollection<string> Countries { get; }
    public IReadOnlyCollection<(string name, string? description)> Actors { get; }
    public IReadOnlyCollection<string> Directors { get; }
    public IReadOnlyCollection<string> Genres { get; }
    public IReadOnlyCollection<string> Screenwriters { get; }
}