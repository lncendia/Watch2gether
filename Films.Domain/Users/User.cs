using System;
using System.Collections.Generic;
using System.Linq;
using Films.Domain.Abstractions;
using Films.Domain.Extensions;
using Films.Domain.Films;
using Films.Domain.Users.ValueObjects;

namespace Films.Domain.Users;

/// <summary>
/// Класс, представляющий пользователя.
/// </summary>
public class User : AggregateRoot
{
    private readonly List<FilmNote> _watchlist = [];
    private readonly List<FilmNote> _history = [];
    private readonly List<string> _genres = [];

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public required string UserName { get; set; } //todo: validate

    /// <summary>
    /// URL-адрес фотографии пользователя.
    /// </summary>
    public required Uri PhotoUrl { get; set; }

    /// <summary>
    /// Разрешения пользователя.
    /// </summary>
    public Allows Allows { get; set; } = new()
    {
        Beep = false,
        Scream = false,
        Change = false
    };

    /// <summary>
    /// Коллекция фильмов, добавленных пользователем в "Список желаемого".
    /// </summary>
    public IReadOnlyCollection<FilmNote> Watchlist => _watchlist;

    /// <summary>
    /// Коллекция просмотренных фильмов.
    /// </summary>
    public IReadOnlyCollection<FilmNote> History => _history;

    /// <summary>
    /// Коллекция, содержащая жанры предпочтений пользователя.
    /// </summary>
    public IReadOnlyCollection<string> Genres => _genres;

    /// <summary>
    /// Добавляет фильм в "Список желаемого" пользователя.
    /// </summary>
    /// <param name="film">Фильм, который нужно добавить в список желаемого</param>
    public void AddFilmToWatchlist(Film film)
    {
        _watchlist.RemoveAll(x => x.FilmId == film.Id);
        if (_watchlist.Count > 14) _watchlist.Remove(_watchlist.OrderBy(x => x.Date).First());
        _watchlist.Add(new FilmNote
        {
            FilmId = film.Id
        });
    }

    /// <summary>
    /// Удаляет фильм из "Списока желаемого" пользователя.
    /// </summary>
    /// <param name="filmId">Идентификатор фильма, который нужно удалить из списка желаемого</param>
    public void RemoveFilmFromWatchlist(Guid filmId) => _watchlist.RemoveAll(x => x.FilmId == filmId);

    /// <summary>
    /// Добавляет фильм в историю просмотров пользователя.
    /// </summary>
    /// <param name="film">Фильм, который нужно добавить в историю просмотров</param>
    public void AddFilmToHistory(Film film)
    {
        _history.RemoveAll(x => x.FilmId == film.Id);
        if (_history.Count > 14) _history.Remove(_history.First());
        _history.Add(new FilmNote
        {
            FilmId = film.Id
        });
    }

    /// <summary>
    /// Обновляет жанры предпочтений пользователя.
    /// </summary>
    /// <param name="genres">Коллекция новых жанров, которые предпочитает пользователь.</param>
    public void UpdateGenres(IEnumerable<string> genres)
    {
        _genres.Clear();
        _genres.AddRange(genres.Select(s => s.GetUpper()).Distinct());
    }
}