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
    public CommentDto MapComment(Comment comment, User? user)
    {
        return new CommentDto(user?.Name ?? "Удаленный пользователь", comment.Text, comment.CreatedAt,
            comment.FilmId, user?.AvatarUri ?? ApplicationConstants.DefaultAvatar);
    }

    public RoomDto MapFilmRoom(FilmRoom room, Film film)
    {
        return new RoomDto(room.Id, Type.Film, room.Viewers.Count, film.Name);
    }

    public RoomDto MapYoutubeRoom(YoutubeRoom room)
    {
        return new RoomDto(room.Id, Type.Film, room.Viewers.Count, room.Owner.CurrentVideoId);
    }


    public FilmDto MapFilm(Domain.Films.Entities.Film film)
    {
        return new FilmDto(film.Name, film.PosterUri, film.Id, film.FilmTags.Genres);
    }
}