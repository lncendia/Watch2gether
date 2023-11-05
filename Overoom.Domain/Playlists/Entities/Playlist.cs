using Overoom.Domain.Abstractions;
using Overoom.Domain.Playlists.Events;
using Overoom.Domain.Playlists.Exceptions;

namespace Overoom.Domain.Playlists.Entities;

public class Playlist : AggregateRoot
{
    public Playlist(Uri posterUri, string name, string description)
    {
        PosterUri = posterUri;
        Name = name;
        Description = description;
    }

    private string _name = null!;

    public string Name
    {
        get => _name;
        set
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
    public Uri PosterUri { get; set; }

    private readonly List<Guid> _films = new();
    private readonly List<string> _genres = new();

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