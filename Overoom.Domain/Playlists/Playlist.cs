namespace Overoom.Domain.Playlists;

public class Playlist
{
    public Playlist(string posterFileName, string name, string description)
    {
        PosterFileName = posterFileName;
        Name = name;
        Description = description;
        Id = Guid.NewGuid();
    }

    public Playlist(List<Guid> films, string posterFileName, string name, string description)
    {
        PosterFileName = posterFileName;
        Name = name;
        Description = description;
        films.ForEach(AddFilm);
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public DateTime Updated { get; } = DateTime.UtcNow;
    public string PosterFileName { get; }
    
    private readonly List<Guid> _films = new();

    public List<Guid> Films => _films.ToList();

    public void AddFilm(Guid filmId)
    {
        if (_films.Any(x => x == filmId)) throw new Exception("Film already in playlist");
        _films.Add(filmId);
    }
}