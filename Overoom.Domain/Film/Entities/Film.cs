using Overoom.Domain.Abstractions;
using Overoom.Domain.Film.DTOs;
using Overoom.Domain.Film.Enums;
using Overoom.Domain.Film.ValueObject;

namespace Overoom.Domain.Film.Entities;

public class Film : AggregateRoot
{
    public Film(string name, string description, string? shortDescription, DateOnly date, double rating, FilmType type,
        IEnumerable<CdnDto> cdn, IEnumerable<string> genres, IEnumerable<(string name, string description)> actors,
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
        FilmTags = new FilmTags(genres, countries, actors, directors, screenwriters);

        Type = type;
        Name = name;
        PosterFileName = posterFileName;
        Date = date;

        _cdnList = cdn.Select(x => new Cdn(x.Type, x.Uri, x.Quality, x.Voices)).ToList();
        if (!_cdnList.Any()) throw new ArgumentException("Cdn list is empty");
    }

    public string Name { get; }
    public DateOnly Date { get; }
    public FilmType Type { get; }

    private readonly List<Cdn> _cdnList;
    public IReadOnlyCollection<Cdn> CdnList => _cdnList.AsReadOnly();
    public string PosterFileName { get; }
    public FilmInfo FilmInfo { get; private set; }
    public FilmTags FilmTags { get; }

    private double _userRating;

    public double UserRating
    {
        get => _userRating;
        set
        {
            if (value is < 0 or > 10)
                throw new ArgumentException("Rating must be between 0 and 10");
            _userRating = value;
        }
    }

    public void UpdateInfo(string description, string? shortDescription, double rating, int? countSeasons = null,
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