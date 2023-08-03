using Overoom.Application.Abstractions.Comments.DTOs;
using Overoom.Application.Abstractions.Movie.DTOs;
using Overoom.WEB.Models.Film;

namespace Overoom.WEB.Mappers.Abstractions;

public interface IFilmMapper
{
    public FilmViewModel Map(FilmDto film);
    public CommentViewModel Map(CommentDto comment, Guid? userId);
}