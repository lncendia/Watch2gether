using Overoom.Application.Abstractions.Users.DTOs;
using Overoom.Application.Abstractions.Users.Interfaces;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Ratings;
using Overoom.Domain.Users.Entities;

namespace Overoom.Application.Services.Users;

public class ProfileMapper : IProfileMapper
{
    public ProfileDto Map(User user, IEnumerable<Film> history, IEnumerable<Film> watchlist)
    {
        var watchedFilms = history.Select(x => new FilmDto(x.Name, x.Id, x.Year, x.PosterUri)).ToList();
        var favoriteFilms = watchlist.Select(x => new FilmDto(x.Name, x.Id, x.Year, x.PosterUri)).ToList();
        return new ProfileDto(user.Name, user.Email, user.AvatarUri, watchedFilms, favoriteFilms);
    }

    public RatingDto Map(Rating rating, Film film) => new(film.Name, film.Id, film.Year, rating.Score, film.PosterUri);
}