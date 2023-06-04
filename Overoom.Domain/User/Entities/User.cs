using System.Net.Mail;
using System.Text.RegularExpressions;
using Overoom.Domain.Abstractions;
using Overoom.Domain.User.Exceptions;

namespace Overoom.Domain.User.Entities;

public class User : AggregateRoot
{
    public User(string name, string email, string avatarFileName)
    {
        Name = name;
        _name = name;
        _email = email;
        Email = email;
        AvatarFileName = avatarFileName;
    }
    
    public string AvatarFileName { get; set; }

    private string _email;

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


    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            if (Regex.IsMatch(value, "^[a-zA-Zа-яА-Я0-9_ ]{3,20}$")) _name = value;
            else throw new InvalidNicknameException(value);
        }
    }

    private readonly List<Guid> _favoriteFilms = new();
    private readonly List<Guid> _watchedFilms = new();

    public List<Guid> FavoriteFilms => _favoriteFilms.ToList();
    public List<Guid> WatchedFilms => _watchedFilms.ToList();

    public void AddFavoriteFilm(Guid filmId)
    {
        if (_favoriteFilms.Contains(filmId)) return;
        if (_favoriteFilms.Count > 50) _favoriteFilms.RemoveAt(0);
        _favoriteFilms.Add(filmId);
    }

    public void AddWatchedFilm(Guid filmId)
    {
        if (_watchedFilms.Contains(filmId)) return;
        if (_watchedFilms.Count > 50) _watchedFilms.RemoveAt(0);
        _watchedFilms.Add(filmId);
    }
}