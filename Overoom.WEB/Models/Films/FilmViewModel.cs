using Overoom.Domain.Films.Enums;

namespace Overoom.WEB.Models.Films;

public class FilmViewModel
{
    public FilmViewModel(Guid id, string name, int year, FilmType type, Uri posterUri,
        string description, double rating, IReadOnlyCollection<string> directors,
        IReadOnlyCollection<string> screenWriters, IReadOnlyCollection<string> genres,
        IReadOnlyCollection<string> countries, IReadOnlyCollection<(string name, string? desc)> actors,
        int? countSeasons, int? countEpisodes, IReadOnlyCollection<CdnViewModel> cndList)
    {
        Name = name + " (" + year + ")";
        PosterUri = posterUri;
        Description = description;
        Rating = rating;
        Directors = directors;
        Genres = genres;
        Countries = countries;
        Actors = actors;
        CountSeasons = countSeasons;
        CountEpisodes = countEpisodes;
        CndList = cndList;
        ScreenWriters = screenWriters;
        Id = id;
        TypeString = type switch
        {
            FilmType.Serial => $"Сериал, {countSeasons} сезон(ов), {countEpisodes} эпизод(ов)",
            FilmType.Film => "Фильм",
            _ => throw new NotImplementedException()
        };
    }

    public Guid Id { get; }
    public string Description { get; }
    public string TypeString { get; }
    public string Name { get; }
    public Uri PosterUri { get; }
    public double Rating { get; }
    public IReadOnlyCollection<CdnViewModel> CndList { get; }
    public int? CountSeasons { get; }
    public int? CountEpisodes { get; }

    public IReadOnlyCollection<string> Genres { get; }
    public IReadOnlyCollection<string> Countries { get; }
    public IReadOnlyCollection<string> Directors { get; }
    public IReadOnlyCollection<string> ScreenWriters { get; }
    public IReadOnlyCollection<(string name, string? desc)> Actors { get; }
}