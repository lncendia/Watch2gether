using Films.Domain.Films.Entities;
using Films.Domain.Ratings.Entities;
using Films.Domain.Users.Entities;

namespace Films.Application.Services.Profile;

public class ProfileMapper : IProfileMapper
{
    public ProfileDto Map(User user, IEnumerable<Film> history, IEnumerable<Film> watchlist)
    {
        var watchedFilms = history.Select(x => new FilmDto(x.Title, x.Id, x.Year, x.PosterUri)).ToList();
        var favoriteFilms = watchlist.Select(x => new FilmDto(x.Title, x.Id, x.Year, x.PosterUri)).ToList();
        var allows = new AllowsDto(user.Allows.Beep, user.Allows.Scream, user.Allows.Change);
        return new ProfileDto(user.Name, user.Email, user.AvatarUri, watchedFilms, favoriteFilms, allows);
    }

    public RatingDto Map(Rating rating, Film film) => new(film.Title, film.Id, film.Year, rating.Score, film.PosterUri);
}