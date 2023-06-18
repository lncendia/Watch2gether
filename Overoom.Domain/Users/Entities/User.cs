using System.Net.Mail;
using System.Text.RegularExpressions;
using Overoom.Domain.Abstractions;
using Overoom.Domain.Users.Exceptions;

namespace Overoom.Domain.Users.Entities;

public class User : AggregateRoot
{
    public User(string name, string email, Uri avatarUri)
    {
        Name = name;
        Email = email;
        AvatarUri = avatarUri;
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
                throw new InvalidEmailException(value);
            }
        }
    }


    private string _name = null!;

    public string Name
    {
        get => _name;
        set
        {
            if (Regex.IsMatch(value, "^[a-zA-Zа-яА-Я0-9_ ]{3,20}$")) _name = value;
            else throw new InvalidNicknameException(value);
        }
    }

    private readonly List<Guid> _watchlist = new();
    private readonly List<Guid> _history = new();

    public IReadOnlyCollection<Guid> Watchlist => _watchlist.AsReadOnly();
    public IReadOnlyCollection<Guid> History => _history.AsReadOnly();

    public void AddFilmToWatchlist(Guid filmId)
    {
        if (_watchlist.Contains(filmId)) return;
        if (_watchlist.Count > 50) _watchlist.RemoveAt(0);
        _watchlist.Add(filmId);
    }

    public void AddFilmToHistory(Guid filmId)
    {
        if (_history.Contains(filmId)) return;
        if (_history.Count > 50) _history.RemoveAt(0);
        _history.Add(filmId);
    }
}