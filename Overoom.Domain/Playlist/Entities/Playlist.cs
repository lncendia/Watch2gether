using Overoom.Domain.Abstractions;

namespace Overoom.Domain.Playlist.Entities;

public class Playlist : AggregateRoot
{
    public Playlist(string posterFileName, string name, string description)
    {
        PosterFileName = posterFileName;
        Name = name;
        Description = description;
    }

    public Playlist(List<Guid> films, string posterFileName, string name, string description)
    {
        PosterFileName = posterFileName;
        Name = name;
        Description = description;
        films.ForEach(AddFilm);
    }

    public string Name { get; }
    public string Description { get; }
    public DateTime Updated { get; } = DateTime.UtcNow;
    public string PosterFileName { get; }

    private readonly List<Guid> _films = new();

    public IReadOnlyCollection<Guid> Films => _films.AsReadOnly();

    public void AddFilm(Guid filmId)
    {
        if (_films.Any(x => x == filmId)) throw new Exception("Film already in playlist");
        _films.Add(filmId);
    }
}