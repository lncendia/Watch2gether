using Overoom.Domain.Films.Enums;

namespace Overoom.WEB.Models.FilmManagement;

public class FilmInfoViewModel
{
    public FilmInfoViewModel(string name, string? description, string? shortDescription, double rating, int year,
        FilmType type, Uri? posterUri, IReadOnlyCollection<string> genres,
        IReadOnlyCollection<ActorViewModel> actors,
        IReadOnlyCollection<string> countries, IReadOnlyCollection<string> directors,
        IReadOnlyCollection<string> screenwriters,
        int? countSeasons, int? countEpisodes, IReadOnlyCollection<CdnViewModel> cdn)
    {
        Name = name;
        Year = year;
        Type = type.ToString();
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
    public string Type { get; }
    public Uri? PosterUri { get; }
    public string Name { get; }
    public int Year { get; }
    public double Rating { get; }
    public int? CountSeasons { get; }
    public int? CountEpisodes { get; }

    public IReadOnlyCollection<string> Countries { get; }
    public IReadOnlyCollection<ActorViewModel> Actors { get; }
    public IReadOnlyCollection<string> Directors { get; }
    public IReadOnlyCollection<string> Genres { get; }
    public IReadOnlyCollection<string> Screenwriters { get; }
    public IReadOnlyCollection<CdnViewModel> Cdn { get; }
}