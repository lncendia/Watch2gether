using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.StartPage.DTOs;
using Overoom.Application.Abstractions.StartPage.Interfaces;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Users.Entities;

namespace Overoom.Application.Services.StartPage;

public class StartPageMapper : IStartPageMapper
{
    public CommentStartPageDto MapComment(Domain.Comments.Entities.Comment comment, Domain.Users.Entities.User? user)
    {
        return new CommentStartPageDto(user?.Name ?? "Удаленный пользователь", comment.Text, comment.CreatedAt,
            comment.FilmId, user?.AvatarUri ?? ApplicationConstants.DefaultAvatar);
    }

    public RoomStartPageDto MapRoom()
    {
        throw new NotImplementedException();
    }


    public FilmStartPageDto MapFilm(Film film)
    {
        return new FilmStartPageDto(film.Name, film.PosterUri, film.Id, film.FilmTags.Genres);
    }
}