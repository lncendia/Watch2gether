using Films.Domain.Abstractions;
using Films.Domain.Extensions;
using Films.Domain.Playlists.Events;
using Films.Domain.Playlists.Exceptions;

namespace Films.Domain.Playlists.Entities;

/// <summary>
/// Плейлист фильмов.
/// </summary>
public class Playlist : AggregateRoot
{
    private string _name = null!;

    /// <summary>
    /// Название плейлиста.
    /// </summary>
    public required string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrEmpty(value) || value.Length > 200) throw new NameLengthException();
            _name = value.GetUpper();
        }
    }

    private string _description = null!;

    /// <summary>
    /// Описание плейлиста.
    /// </summary>
    public required string Description
    {
        get => _description;
        set
        {
            if (string.IsNullOrEmpty(value) || value.Length > 500) throw new DescriptionLengthException();
            _description = value.GetUpper();
        }
    }

    /// <summary>
    /// URL-адрес постера плейлиста.
    /// </summary>
    public required Uri PosterUrl { get; set; }

    /// <summary>
    /// Дата обновления плейлиста.
    /// </summary>
    public DateTime Updated { get; } = DateTime.UtcNow;

    private readonly List<Guid> _films = [];
    private readonly List<string> _genres = [];

    /// <summary>
    /// Коллекция содержащая идентификаторы фильмов в плейлисте.
    /// </summary>
    public IReadOnlyCollection<Guid> Films => _films;

    /// <summary>
    /// Коллекция, содержащая жанры плейлиста.
    /// </summary>
    public IReadOnlyCollection<string> Genres => _genres;

    /// <summary>
    /// Обновляет список фильмов в плейлисте.
    /// </summary>
    /// <param name="filmsIds">Коллекция идентификаторов фильмов, которые нужно добавить или удалить из плейлиста.</param>
    public void UpdateFilms(IEnumerable<Guid> filmsIds)
    {
        var updateCollection = filmsIds.Distinct().ToList();

        var newFilms = updateCollection.Where(x => !_films.Contains(x));

        var removeFilms = _films.Where(x => !updateCollection.Contains(x)).ToList();

        _films.RemoveAll(removeFilms.Contains);
        _films.AddRange(newFilms);
        AddDomainEvent(new FilmsCollectionUpdateEvent(this));
    }

    /// <summary>
    /// Обновляет жанры плейлиста.
    /// </summary>
    /// <param name="genres">Коллекция новых жанров плейлиста.</param>
    public void UpdateGenres(IEnumerable<string> genres)
    {
        _genres.Clear();
        _genres.AddRange(genres.Select(s=>s.GetUpper()).Distinct());
    }
}