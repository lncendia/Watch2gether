using Films.Domain.Abstractions;
using Films.Domain.Films.Enums;
using Films.Domain.Films.Events;
using Films.Domain.Films.Exceptions;
using Films.Domain.Films.ValueObjects;
using Films.Domain.Ratings.Entities;

namespace Films.Domain.Films.Entities;

public class Film : AggregateRoot
{
    public Film(FilmType type, int? countSeasons = null, int? countEpisodes = null)
    {
        Type = type;
        if (type == FilmType.Serial)
        {
            if (countSeasons == null || countEpisodes == null) throw new SerialException();
            CountSeasons = countSeasons;
            CountEpisodes = countEpisodes;
        }

        AddDomainEvent(new NewFilmEvent(this));
    }


    #region Info

    private readonly string _title = null!;
    private string _description = null!;
    private string? _shortDescription;

    public required string Title
    {
        get => _title;
        init
        {
            if (string.IsNullOrEmpty(value) || value.Length > 200) throw new NameLengthException();
            _title = value;
        }
    }

    public required string Description
    {
        get => _description;
        set
        {
            if (string.IsNullOrEmpty(value) || value.Length > 1500) throw new DescriptionLengthException();
            _description = value;
        }
    }

    public string? ShortDescription
    {
        get => string.IsNullOrEmpty(_shortDescription) ? _description[..100] + "..." : _shortDescription;
        set
        {
            if (value?.Length > 500) throw new ShortDescriptionLengthException();
            _shortDescription = value;
        }
    }

    public required int Year { get; init; }
    public required Uri PosterUrl { get; set; }

    #endregion

    #region Collections

    private readonly string[] _countries = null!;
    private readonly string[] _directors = null!;
    private readonly string[] _screenwriters = null!;
    private readonly Actor[] _actors = null!;
    private readonly string[] _genres = null!;

    public required IReadOnlyCollection<string> Genres
    {
        get => _genres.AsReadOnly();
        init
        {
            if (value.Count == 0) throw new EmptyTagsCollectionException();
            _genres = value.ToArray();
        }
    }

    public required IReadOnlyCollection<string> Countries
    {
        get => _countries.AsReadOnly();
        init
        {
            if (value.Count == 0) throw new EmptyTagsCollectionException();
            _countries = value.ToArray();
        }
    }

    public required IReadOnlyCollection<string> Directors
    {
        get => _directors.AsReadOnly();
        init
        {
            if (value.Count == 0) throw new EmptyTagsCollectionException();
            _directors = value.ToArray();
        }
    }

    public required IReadOnlyCollection<Actor> Actors
    {
        get => _actors.AsReadOnly();
        init
        {
            if (value.Count == 0) throw new EmptyTagsCollectionException();
            _actors = value.ToArray();
        }
    }

    public required IReadOnlyCollection<string> Screenwriters
    {
        get => _screenwriters.AsReadOnly();
        init
        {
            if (value.Count == 0) throw new EmptyTagsCollectionException();
            _screenwriters = value.ToArray();
        }
    }

    #endregion

    #region Rating

    public double UserRating { get; private set; }
    public int UserRatingsCount { get; private set; }

    public void AddRating(Rating rating, Rating? oldRating)
    {
        var allScore = UserRating * UserRatingsCount;
        if (oldRating == null) UserRatingsCount++;
        else allScore -= oldRating.Score;
        UserRating = (allScore + rating.Score) / UserRatingsCount;
    }


    private double? _ratingKp;

    private double? _ratingImdb;

    public double? RatingKp
    {
        get => _ratingKp;
        set
        {
            if (value is < 0 or > 10)
                throw new ArgumentOutOfRangeException(nameof(value), "Rating must be between 0 and 10");
            _ratingKp = value;
        }
    }

    public double? RatingImdb
    {
        get => _ratingImdb;
        set
        {
            if (value is < 0 or > 10)
                throw new ArgumentOutOfRangeException(nameof(value), "Rating must be between 0 and 10");
            _ratingImdb = value;
        }
    }

    #endregion

    #region Type

    public FilmType Type { get; }
    public int? CountSeasons { get; private set; }
    public int? CountEpisodes { get; private set; }

    public void UpdateSeriesInfo(int countSeasons, int countEpisodes)
    {
        if (Type != FilmType.Serial) throw new NotSerialException();

        CountSeasons = countSeasons;
        CountEpisodes = countEpisodes;
    }

    #endregion

    #region Cdn

    private readonly List<Cdn> _cdnList = null!;

    public required IReadOnlyCollection<Cdn> CdnList
    {
        get => _cdnList.AsReadOnly();
        init
        {
            if (value.Count == 0) throw new EmptyCdnsCollectionException();
            if (_cdnList.GroupBy(x => x.Type).Any(x => x.Count() > 1)) throw new DuplicateCdnException();
            _cdnList = value.ToList();
        }
    }

    public void AddOrChangeCdn(Cdn cdn)
    {
        _cdnList.RemoveAll(x => x.Type == cdn.Type);
        _cdnList.Add(cdn);
    }

    #endregion
}