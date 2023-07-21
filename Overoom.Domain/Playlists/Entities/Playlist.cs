using Overoom.Domain.Abstractions;
using Overoom.Domain.Playlists.Exceptions;

namespace Overoom.Domain.Playlists.Entities;

public class Playlist : AggregateRoot
{
    public Playlist(Uri posterUri, string name, string description, IEnumerable<string> genres)
    {
        PosterUri = posterUri;
        Name = name;
        Description = description;
        _genres = genres.Distinct().ToList();
        if (!_genres.Any()) throw new EmptyGenresCollectionException();
    }

    private readonly string _name = null!;

    public string Name
    {
        get => _name;
        private init
        {
            if (string.IsNullOrEmpty(value) || value.Length > 200) throw new NameLengthException();
            _name = value;
        }
    }

    private string _description = null!;

    public string Description
    {
        get => _description;
        set
        {
            if (string.IsNullOrEmpty(value) || value.Length > 500) throw new DescriptionLengthException();
            _description = value;
        }
    }

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