using Overoom.Domain.Films.Enums;

namespace Overoom.WEB.Models.Film;

public class FilmViewModel
{
    public FilmViewModel(Guid id, string name, int year, FilmType type, string posterFileName,
        string description, double rating, List<string> directors, List<string> screenWriters, List<string> genres,
        List<string> countries, List<(string name, string desc)> actors, int? countSeasons, int? countEpisodes, string url)
    {
        Name = name + " (" + year + ")";
        PosterFileName = posterFileName;
        Description = description;
        Rating = rating;
        Directors = directors;
        Genres = genres;
        Countries = countries;
        Actors = actors;
        CountSeasons = countSeasons;
        CountEpisodes = countEpisodes;
        Url = url;
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
    public string PosterFileName { get; }
    public double Rating { get; }
    public string Url { get; }
    public int? CountSeasons { get; }
    public int? CountEpisodes { get; }

    public List<string> Genres { get; }
    public List<string> Countries { get; }
    public List<string> Directors { get; }
    public List<string> ScreenWriters { get; }
    public List<(string name, string desc)> Actors { get; }
    
}