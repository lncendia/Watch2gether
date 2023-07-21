using Overoom.Application.Abstractions.Authentication.DTOs;
using Overoom.Application.Abstractions.Profile.DTOs;
using Overoom.Application.Abstractions.Profile.Interfaces;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Ratings;
using Overoom.Domain.Ratings.Entities;
using Overoom.Domain.Users.Entities;

namespace Overoom.Application.Services.Profile;

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