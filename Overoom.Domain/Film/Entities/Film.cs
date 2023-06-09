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
        string posterUri, int? countSeasons = null, int? countEpisodes = null)
    {
        FilmTags = new FilmTags(genres, countries, actors, directors, screenwriters);

        Type = type;
        Name = name;
        PosterUri = posterUri;
        Date = date;
        RatingKp = rating;
        Description = description;
        ShortDescription = shortDescription;
        Type = type;
        if (countSeasons != null && countEpisodes != null) UpdateSeriesInfo(countSeasons.Value, countEpisodes.Value);

        _cdnList = cdn.Select(x => new Cdn(x.Type, x.Uri, x.Quality, x.Voices)).ToList();
        if (!_cdnList.Any()) throw new ArgumentException("Cdn list is empty");
    }

    public string Name { get; }
    public DateOnly Date { get; }
    public FilmType Type { get; }

    private readonly List<Cdn> _cdnList;
    public IReadOnlyCollection<Cdn> CdnList => _cdnList.AsReadOnly();
    public string PosterUri { get; }
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

    public string Description { get; }
    public string? ShortDescription { get; }
    public int? CountSeasons { get; private set; }
    public int? CountEpisodes { get; private set; }

    private readonly double _rating;

    public double RatingKp
    {
        get => _rating;
        private init
        {
            if (value is < 0 or > 10)
                throw new ArgumentException("Rating must be between 0 and 10");
            _rating = value;
        }
    }

    public void UpdateSeriesInfo(int countSeasons, int countEpisodes)
    {
        if (Type != FilmType.Serial)
            throw new InvalidOperationException("Count of episodes must be specified for serials");

        CountSeasons = countSeasons;
        CountEpisodes = countEpisodes;
    }
}