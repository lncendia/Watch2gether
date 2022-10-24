using Overoom.Domain.Films.Enums;
using Overoom.Domain.Films.ValueObject;

namespace Overoom.Domain.Films;

public class Film
{
    public Film(string name, string? description, string? shortDescription, DateOnly date, double rating, FilmType type,
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

        FilmInfo = new FilmInfo(rating, description, shortDescription, countSeasons, countEpisodes);
        FilmCollections = new FilmCollections(genres, countries, actors, directors, screenwriters);

        Type = type;
        Name = name;
        Url = url;
        PosterFileName = posterFileName;
        Date = date;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
    public string Name { get; }
    public DateOnly Date { get; }
    public FilmType Type { get; }
    public Uri Url { get; }
    public string PosterFileName { get; }
    public FilmInfo FilmInfo { get; private set; }
    public FilmCollections FilmCollections { get; }

    public void UpdateInfo(string? description, string? shortDescription, double rating, int? countSeasons = null,
        int? countEpisodes = null)
    {
        switch (Type)
        {
            case FilmType.Serial when countSeasons == null:
                throw new ArgumentException("Count of seasons must be specified for serials");
            case FilmType.Serial when countEpisodes == null:
                throw new ArgumentException("Count of episodes must be specified for serials");
        }

        FilmInfo = new FilmInfo(rating, description, shortDescription, countSeasons, countEpisodes);
    }
}