using Overoom.Application.Abstractions;
using Overoom.Application.Abstractions.StartPage.DTOs;
using Overoom.Application.Abstractions.StartPage.Interfaces;
using Overoom.Domain.Comments.Entities;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Rooms.FilmRoom.Entities;
using Overoom.Domain.Rooms.YoutubeRoom.Entities;
using Overoom.Domain.Users.Entities;
using Type = Overoom.Application.Abstractions.StartPage.DTOs.Type;

namespace Overoom.Application.Services.StartPage;

public class StartPageMapper : IStartPageMapper
{
    public CommentStartPageDto MapComment(Comment comment, User? user)
    {
        return new CommentStartPageDto(user?.Name ?? "Удаленный пользователь", comment.Text, comment.CreatedAt,
            comment.FilmId, user?.AvatarUri ?? ApplicationConstants.DefaultAvatar);
    }

    public RoomStartPageDto MapFilmRoom(FilmRoom room, Film film)
    {
        return new RoomStartPageDto(room.Id, Type.Film, room.Viewers.Count, film.Name);
    }

    public RoomStartPageDto MapYoutubeRoom(YoutubeRoom room)
    {
        return new RoomStartPageDto(room.Id, Type.Film, room.Viewers.Count, room.Owner.CurrentVideoId);
    }


    public FilmStartPageDto MapFilm(Domain.Films.Entities.Film film)
    {
        return new FilmStartPageDto(film.Name, film.PosterUri, film.Id, film.FilmTags.Genres);
    }
}