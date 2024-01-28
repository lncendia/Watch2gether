namespace Films.Infrastructure.Web.Models.Settings;

public class ProfileViewModel
{
    public ProfileViewModel(string name, string email, Uri avatar, IReadOnlyCollection<FilmViewModel> watchedFilms,
        IReadOnlyCollection<FilmViewModel> favoriteFilms, IReadOnlyCollection<string> genres, AllowsViewModel allows)
    {
        Name = name;
        Email = email;
        Avatar = avatar;
        WatchedFilms = watchedFilms;
        FavoriteFilms = favoriteFilms;
        Genres = genres;
        Allows = allows;
    }

    public string Name { get; }
    public string Email { get; }
    public Uri Avatar { get; }
    public AllowsViewModel Allows { get; }
    public IReadOnlyCollection<FilmViewModel> WatchedFilms { get; }
    public IReadOnlyCollection<FilmViewModel> FavoriteFilms { get; }
    public IReadOnlyCollection<string> Genres { get; }
}