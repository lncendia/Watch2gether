using Overoom.Domain.Films.Enums;

namespace Overoom.Application.Abstractions.Films.Catalog.DTOs;

public class FilmDtoBuilder
{
    private FilmDtoBuilder()
    {
    }

    private Guid? _id;
    private string? _name;
    private string? _description;
    private int? _year;
    private double? _rating;
    private double? _userRating;
    private FilmType? _type;
    private IReadOnlyCollection<CdnDto>? _cdnList;
    private IReadOnlyCollection<string>? _genres;
    private IReadOnlyCollection<(string name, string? description)>? _actors;
    private IReadOnlyCollection<string>? _directors;
    private IReadOnlyCollection<string>? _screenwriters;
    private IReadOnlyCollection<string>? _countries;
    private Uri? _posterUri;
    private int? _countSeasons;
    private int? _countEpisodes;

    public static FilmDtoBuilder Create() => new();
    
    public FilmDtoBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public FilmDtoBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public FilmDtoBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public FilmDtoBuilder WithYear(int year)
    {
        _year = year;
        return this;
    }

    public FilmDtoBuilder WithRating(double rating)
    {
        _rating = rating;
        return this;
    }

    public FilmDtoBuilder WithUserRating(double rating)
    {
        _userRating = rating;
        return this;
    }

    public FilmDtoBuilder WithType(FilmType type)
    {
        _type = type;
        return this;
    }

    public FilmDtoBuilder WithCdn(IReadOnlyCollection<CdnDto> cdn)
    {
        _cdnList = cdn;
        return this;
    }

    public FilmDtoBuilder WithGenres(IReadOnlyCollection<string> genres)
    {
        _genres = genres;
        return this;
    }

    public FilmDtoBuilder WithActors(IReadOnlyCollection<(string name, string? description)> actors)
    {
        _actors = actors;
        return this;
    }

    public FilmDtoBuilder WithDirectors(IReadOnlyCollection<string> directors)
    {
        _directors = directors;
        return this;
    }

    public FilmDtoBuilder WithScreenwriters(IReadOnlyCollection<string> screenwriters)
    {
        _screenwriters = screenwriters;
        return this;
    }

    public FilmDtoBuilder WithCountries(IReadOnlyCollection<string> countries)
    {
        _countries = countries;
        return this;
    }

    public FilmDtoBuilder WithPoster(Uri uri)
    {
        _posterUri = uri;
        return this;
    }

    public FilmDtoBuilder WithEpisodes(int countSeasons, int countEpisodes)
    {
        _countSeasons = countSeasons;
        _countEpisodes = countEpisodes;
        return this;
    }

    public FilmDto Build()
    {
        if (string.IsNullOrEmpty(_name)) throw new InvalidOperationException("builder not formed");
        if (string.IsNullOrEmpty(_description)) throw new InvalidOperationException("builder not formed");
        if (_id == null) throw new InvalidOperationException("builder not formed");
        if (_year == null) throw new InvalidOperationException("builder not formed");
        if (_rating == null) throw new InvalidOperationException("builder not formed");
        if (_userRating == null) throw new InvalidOperationException("builder not formed");
        if (_type == null) throw new InvalidOperationException("builder not formed");
        if (_cdnList == null) throw new InvalidOperationException("builder not formed");
        if (_posterUri == null) throw new InvalidOperationException("builder not formed");
        if (_genres == null) throw new InvalidOperationException("builder not formed");
        if (_actors == null) throw new InvalidOperationException("builder not formed");
        if (_directors == null) throw new InvalidOperationException("builder not formed");
        if (_screenwriters == null) throw new InvalidOperationException("builder not formed");
        if (_countries == null) throw new InvalidOperationException("builder not formed");

        return new FilmDto(_id.Value, _name, _year.Value, _type.Value, _posterUri, _description, _rating.Value,
            _userRating.Value, _directors, _screenwriters, _genres, _countries, _actors, _countSeasons, _countEpisodes,
            _cdnList);
    }
}