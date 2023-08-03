using Overoom.Application.Abstractions.Authentication.DTOs;

namespace Overoom.Application.Abstractions.Profile.DTOs;

public class ProfileDto
{
    public ProfileDto(string name, string email, Uri avatar, IReadOnlyCollection<FilmDto> watchedFilms,
        IReadOnlyCollection<FilmDto> favoriteFilms, AllowsDto allows)
    {
        Name = name;
        Email = email;
        Avatar = avatar;
        WatchedFilms = watchedFilms;
        FavoriteFilms = favoriteFilms;
        Allows = allows;
    }

    public string Name { get; }
    public string Email { get; }
    public Uri Avatar { get; }
    public IReadOnlyCollection<FilmDto> WatchedFilms { get; }
    public IReadOnlyCollection<FilmDto> FavoriteFilms { get; }
    public AllowsDto Allows { get; }
}