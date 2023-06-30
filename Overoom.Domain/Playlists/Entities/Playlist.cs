using Overoom.Domain.Abstractions;

namespace Overoom.Domain.Playlists.Entities;

public class Playlist : AggregateRoot
{
    public Playlist(Uri posterUri, string name, string description, IEnumerable<string> genres)
    {
        PosterUri = posterUri;
        Name = name;
        Description = description;
        _genres = genres.Distinct().ToList();
    }

    public string Name { get; }
    public string Description { get; }
    public DateTime Updated { get; } = DateTime.UtcNow;
    public Uri PosterUri { get; }

    private readonly List<Guid> _films = new();
    private readonly List<string> _genres;

    public IReadOnlyCollection<Guid> Films => _films.AsReadOnly();
    public IReadOnlyCollection<string> Genres => _genres.AsReadOnly();

    public void AddFilm(Guid filmId)
    {
        if (_films.Any(x => x == filmId)) throw new Exception("Film already in playlist"); //todo:exception
        _films.Add(filmId);
    }
}