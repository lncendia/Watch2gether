using System.Net.Mail;
using System.Text.RegularExpressions;
using Overoom.Domain.Abstractions;
using Overoom.Domain.Users.Exceptions;
using Overoom.Domain.Users.ValueObjects;

namespace Overoom.Domain.Users.Entities;

public partial class User : AggregateRoot
{
    public User(string name, string email, Uri avatarUri)
    {
        Name = name;
        Email = email;
        AvatarUri = avatarUri;
        Allows = new Allows(true, true, true);
    }

    public Uri AvatarUri { get; set; }

    private string _email = null!;

    public string Email
    {
        get => _email;
        set
        {
            try
            {
                _email = new MailAddress(value).Address;
            }
            catch (FormatException)
            {
                throw new EmailFormatException();
            }
        }
    }


    private string _name = null!;

    public string Name
    {
        get => _name;
        set
        {
            if (value.Length is < 3 or > 20) throw new NicknameLengthException();
            if (MyRegex().IsMatch(value)) _name = value;
            else throw new NicknameFormatException();
        }
    }

    public Allows Allows { get; private set; }

    private readonly List<FilmNote> _watchlist = new();
    private readonly List<FilmNote> _history = new();
    private readonly List<string> _genres = new();

    public IReadOnlyCollection<FilmNote> Watchlist => _watchlist.AsReadOnly();
    public IReadOnlyCollection<FilmNote> History => _history.AsReadOnly();
    public IReadOnlyCollection<string> Genres => _genres.AsReadOnly();

    public void AddFilmToWatchlist(Guid filmId)
    {
        _watchlist.RemoveAll(x => x.FilmId == filmId);
        if (_watchlist.Count > 14) _watchlist.Remove(_watchlist.OrderBy(x => x.Date).First());
        _watchlist.Add(new FilmNote(filmId));
    }

    public void RemoveFilmFromWatchlist(Guid filmId) => _watchlist.RemoveAll(x => x.FilmId == filmId);

    public void AddFilmToHistory(Guid filmId)
    {
        _history.RemoveAll(x => x.FilmId == filmId);
        if (_history.Count > 14) _history.Remove(_history.First());
        _history.Add(new FilmNote(filmId));
    }

    public void UpdateGenres(IEnumerable<string> genres)
    {
        _genres.Clear();
        _genres.AddRange(genres.Distinct());
    }

    public void UpdateAllows(bool beep, bool scream, bool change)
    {
        Allows = new Allows(beep, scream, change);
    }

    [GeneratedRegex("^[a-zA-Zа-яА-Я0-9_ ]+$")]
    private static partial Regex MyRegex();
}