using Overoom.Domain.Films.Exceptions;

namespace Overoom.Domain.Films.ValueObject;

public class FilmTags
{
    internal FilmTags(IEnumerable<string> genres, IEnumerable<string> countries,
        IEnumerable<(string name, string description)> actors, IEnumerable<string> screenwriters,
        IEnumerable<string> directors)
    {
        _genres = genres.Distinct().ToList();
        _countries = countries.Distinct().ToList();
        _actors = actors.Distinct().Select(x => new ActorData(x.name, x.description)).ToList();
        _directors = directors.Distinct().ToList();
        _screenwriters = screenwriters.Distinct().ToList();

        if (_genres.Any()) throw new EmptyTagsCollectionException();
        if (_actors.Any()) throw new EmptyTagsCollectionException();
        if (_directors.Any()) throw new EmptyTagsCollectionException();
        if (_screenwriters.Any()) throw new EmptyTagsCollectionException();
        if (_countries.Any()) throw new EmptyTagsCollectionException();
    }

    private readonly List<string> _countries;
    private readonly List<string> _directors;
    private readonly List<string> _screenwriters;
    private readonly List<ActorData> _actors;
    private readonly List<string> _genres;

    public IReadOnlyCollection<string> Genres => _genres.AsReadOnly();
    public IReadOnlyCollection<string> Countries => _countries.AsReadOnly();
    public IReadOnlyCollection<string> Directors => _directors.AsReadOnly();
    public IReadOnlyCollection<ActorData> Actors => _actors.AsReadOnly();
    public IReadOnlyCollection<string> Screenwriters => _screenwriters.AsReadOnly();
}