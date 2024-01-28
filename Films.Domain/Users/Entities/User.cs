using Films.Domain.Abstractions;
using Films.Domain.Films.Entities;
using Films.Domain.Users.ValueObjects;

namespace Films.Domain.Users.Entities;

public class User : AggregateRoot
{
    public required string UserName { get; set; }

    public required Uri PhotoUrl { get; set; }

    public Allows Allows { get; private set; } = new()
    {
        Beep = false,
        Scream = false,
        Change = false
    };

    private readonly List<FilmNote> _watchlist = [];
    private readonly List<FilmNote> _history = [];
    private readonly List<string> _genres = [];

    public IReadOnlyCollection<FilmNote> Watchlist => _watchlist.AsReadOnly();
    public IReadOnlyCollection<FilmNote> History => _history.AsReadOnly();
    public IReadOnlyCollection<string> Genres => _genres.AsReadOnly();

    public void AddFilmToWatchlist(Film film)
    {
        _watchlist.RemoveAll(x => x.FilmId == film.Id);
        if (_watchlist.Count > 14) _watchlist.Remove(_watchlist.OrderBy(x => x.Date).First());
        _watchlist.Add(new FilmNote(film.Id));
    }

    public void RemoveFilmFromWatchlist(Guid filmId) => _watchlist.RemoveAll(x => x.FilmId == filmId);

    public void AddFilmToHistory(Film film)
    {
        _history.RemoveAll(x => x.FilmId == film.Id);
        if (_history.Count > 14) _history.Remove(_history.First());
        _history.Add(new FilmNote(film.Id));
    }

    public void UpdateGenres(IEnumerable<string> genres)
    {
        _genres.Clear();
        _genres.AddRange(genres.Distinct());
    }

    public void UpdateAllows(bool beep, bool scream, bool change)
    {
        Allows = new Allows
        {
            Beep = beep,
            Scream = scream,
            Change = change
        };
    }
}