using Watch2gether.Domain.Films.Enums;
using Watch2gether.Domain.Films.ValueObject;

namespace Watch2gether.Domain.Films;

public class Film
{
    public Film(string name, string? description, string? shortDescription, int year, double rating, FilmType type,
        Uri url, IEnumerable<string> genres, IEnumerable<(string name, string description)> actors,
        IEnumerable<string> directors, IEnumerable<string> screenwriters, IEnumerable<string> countries,
        string posterFileName, int? countSeasons = null, int? countEpisodes = null)
    {
        switch (Type)
        {
            case FilmType.Serial when countSeasons == null:
                throw new ArgumentException("Count of seasons must be specified for serials");
            case FilmType.Serial when countEpisodes == null:
                throw new ArgumentException("Count of episodes must be specified for serials");
        }

        FilmData = new FilmData(rating, name, description, shortDescription, genres, year, countries, actors,
            screenwriters, directors, countSeasons, countEpisodes);
        Type = type;
        Url = url;
        PosterFileName = posterFileName;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
    public FilmType Type { get; }
    public Uri Url { get; }
    public string PosterFileName { get; }
    public FilmData FilmData { get; }
}