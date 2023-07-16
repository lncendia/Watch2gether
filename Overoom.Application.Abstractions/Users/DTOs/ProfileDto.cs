namespace Overoom.Application.Abstractions.Users.DTOs;

public class ProfileDto
{
    public ProfileDto(string name, string email, Uri avatar, IReadOnlyCollection<FilmDto> watchedFilms,
        IReadOnlyCollection<FilmDto> favoriteFilms)
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
    public IReadOnlyCollection<FilmDto> WatchedFilms { get; }
    public IReadOnlyCollection<FilmDto> FavoriteFilms { get; }
}