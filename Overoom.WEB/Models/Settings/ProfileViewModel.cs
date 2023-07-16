namespace Overoom.WEB.Models.Settings;

public class ProfileViewModel
{
    public ProfileViewModel(string name, string email, Uri avatar, IReadOnlyCollection<FilmViewModel> watchedFilms,
        IReadOnlyCollection<FilmViewModel> favoriteFilms)
    {
        Name = name;
        Email = email;
        Avatar = avatar;
        WatchedFilms = watchedFilms;
        FavoriteFilms = favoriteFilms;
    }

    public string Name { get; }
    public string Email { get; }
    public Uri Avatar { get; }
    public IReadOnlyCollection<FilmViewModel> WatchedFilms { get; }
    public IReadOnlyCollection<FilmViewModel> FavoriteFilms { get; }
}