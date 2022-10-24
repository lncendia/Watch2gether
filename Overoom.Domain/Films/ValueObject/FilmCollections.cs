namespace Overoom.Domain.Films.ValueObject;

public class FilmCollections
{
    public FilmCollections(IEnumerable<string> genres, IEnumerable<string> countries,
        IEnumerable<(string name, string description)> actors, IEnumerable<string> screenwriters,
        IEnumerable<string> directors)
    {
        _genres = genres.ToList();
        _countries = countries.ToList();
        _actors = actors.Select(x => new ActorData(x.name, x.description)).ToList();
        _directors = directors.ToList();
        _screenwriters = screenwriters.ToList();
    }

    private readonly List<string> _countries;
    private readonly List<string> _directors;
    private readonly List<string> _screenwriters;
    private readonly List<ActorData> _actors;
    private readonly List<string> _genres;

    public List<string> Genres => _genres.ToList();
    public List<string> Countries => _countries.ToList();
    public List<string> Directors => _directors.ToList();
    public List<ActorData> Actors => _actors.ToList();
    public List<string> Screenwriters => _screenwriters.ToList();
}