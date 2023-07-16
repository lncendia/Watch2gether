using Overoom.Application.Abstractions.StartPage.DTOs;
using Overoom.Domain.Comments.Entities;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Rooms.FilmRoom.Entities;
using Overoom.Domain.Rooms.YoutubeRoom.Entities;
using Overoom.Domain.Users.Entities;

namespace Overoom.Application.Abstractions.StartPage.Interfaces;

public interface IStartPageMapper
{
    CommentDto MapComment(Comment comment, User? user);

    RoomDto MapFilmRoom(FilmRoom room, Film film);
    RoomDto MapYoutubeRoom(YoutubeRoom room);
    FilmDto MapFilm(Film film);
}