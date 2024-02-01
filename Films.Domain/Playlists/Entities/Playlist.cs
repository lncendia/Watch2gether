using Films.Domain.Abstractions;
using Films.Domain.Playlists.Events;
using Films.Domain.Playlists.Exceptions;

namespace Films.Domain.Playlists.Entities;

public class Playlist : AggregateRoot
{
    private string _name = null!;

    public required string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrEmpty(value) || value.Length > 200) throw new NameLengthException();
            _name = value;
        }
    }

    private string _description = null!;

    public required string Description
    {
        get => _description;
        set
        {
            if (string.IsNullOrEmpty(value) || value.Length > 500) throw new DescriptionLengthException();
            _description = value;
        }
    }

    public required Uri PosterUrl { get; set; }

    public DateTime Updated { get; } = DateTime.UtcNow;

    private readonly List<Guid> _films = [];
    private readonly List<string> _genres = [];

    public IReadOnlyCollection<Guid> Films => _films.AsReadOnly();
    public IReadOnlyCollection<string> Genres => _genres.AsReadOnly();

    public void UpdateFilms(IEnumerable<Guid> filmsIds)
    {
        var updateCollection = filmsIds.Distinct().ToList();

        var newFilms = updateCollection.Where(x => !_films.Contains(x));

        var removeFilms = _films.Where(x => !updateCollection.Contains(x)).ToList();

        _films.RemoveAll(removeFilms.Contains);
        _films.AddRange(newFilms);
        AddDomainEvent(new FilmsCollectionUpdateEvent(this));
    }

    public void UpdateGenres(IEnumerable<string> genres)
    {
        _genres.Clear();
        _genres.AddRange(genres.Distinct());
    }
}