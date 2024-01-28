using Films.Application.Abstractions.Movie.DTOs;
using Films.Application.Abstractions.Queries.Comments.DTOs;
using Films.Application.Abstractions.Queries.Films.DTOs;
using Films.Infrastructure.Web.Models.Film;

namespace Films.Infrastructure.Web.Mappers.Abstractions;

public interface IFilmMapper
{
    public FilmViewModel Map(FilmDto film);
    public CommentViewModel Map(CommentDto comment, Guid? userId);
}