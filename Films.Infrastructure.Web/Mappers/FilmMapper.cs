using Films.Application.Abstractions.Movie.DTOs;
using Films.Application.Abstractions.Queries.Comments.DTOs;
using Films.Application.Abstractions.Queries.Films.DTOs;
using Films.Infrastructure.Web.Mappers.Abstractions;
using Films.Infrastructure.Web.Models.Film;
using Film_FilmViewModel = Films.Infrastructure.Web.Models.Film.FilmViewModel;
using FilmViewModel = Films.Infrastructure.Web.Models.Film.FilmViewModel;
using Models_Film_FilmViewModel = Films.Infrastructure.Web.Models.Film.FilmViewModel;

namespace Films.Infrastructure.Web.Mappers;

public class FilmMapper : IFilmMapper
{
    public Models_Film_FilmViewModel Map(FilmDto film)
    {
        var cdnList = film.CdnList.Select(x => new CdnViewModel(x.Type, x.Voices, x.Quality)).ToList();
        return new Models_Film_FilmViewModel(film.Id, film.Name, film.Year, film.Type, film.PosterUri, film.Description,
            film.Rating, film.Directors, film.ScreenWriters, film.Genres, film.Countries, film.Actors,
            film.CountSeasons, film.CountEpisodes, cdnList, film.UserRating, film.UserScore, film.InWatchlist,
            film.UserRatingsCount);
    }

    public CommentViewModel Map(CommentDto comment, Guid? userId) =>
        new(comment.Username, comment.Text, comment.CreatedAt, comment.AvatarUri, comment.Id, comment.UserId == userId);
}