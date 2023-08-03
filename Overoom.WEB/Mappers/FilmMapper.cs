using Overoom.Application.Abstractions.Comments.DTOs;
using Overoom.Application.Abstractions.Movie.DTOs;
using Overoom.WEB.Mappers.Abstractions;
using Overoom.WEB.Models.Film;
using FilmViewModel = Overoom.WEB.Models.Film.FilmViewModel;

namespace Overoom.WEB.Mappers;

public class FilmMapper : IFilmMapper
{
    public FilmViewModel Map(FilmDto film)
    {
        var cdnList = film.CdnList.Select(x => new CdnViewModel(x.Type, x.Voices, x.Quality)).ToList();
        return new FilmViewModel(film.Id, film.Name, film.Year, film.Type, film.PosterUri, film.Description,
            film.Rating, film.Directors, film.ScreenWriters, film.Genres, film.Countries, film.Actors,
            film.CountSeasons, film.CountEpisodes, cdnList, film.UserRating, film.UserScore, film.InWatchlist,
            film.UserRatingsCount);
    }

    public CommentViewModel Map(CommentDto comment, Guid? userId) =>
        new(comment.Username, comment.Text, comment.CreatedAt, comment.AvatarUri, comment.Id, comment.UserId == userId);
}