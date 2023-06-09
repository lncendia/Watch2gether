using Overoom.Domain.Abstractions;

namespace Overoom.Domain.Playlist.Entities;

public class Playlist : AggregateRoot
{
    public Playlist(string posterUri, string name, string description)
    {
        PosterUri = posterUri;
        Name = name;
        Description = description;
    }

    public Playlist(IEnumerable<Guid> films, string posterUri, string name, string description)
    {
        PosterUri = posterUri;
        Name = name;
        Description = description;
        foreach (var film in films) AddFilm(film);
    }

    public string Name { get; }
    public string Description { get; }
    public DateTime Updated { get; } = DateTime.UtcNow;
    public string PosterUri { get; }

    private readonly List<Guid> _films = new();

    public IReadOnlyCollection<Guid> Films => _films.AsReadOnly();

    public void AddFilm(Guid filmId)
    {
        if (_films.Any(x => x == filmId)) throw new Exception("Film already in playlist"); //todo:exception
        _films.Add(filmId);
    }
}