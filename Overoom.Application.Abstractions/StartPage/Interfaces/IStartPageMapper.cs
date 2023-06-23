using Overoom.Application.Abstractions.StartPage.DTOs;
using Overoom.Domain.Comments.Entities;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Rooms.FilmRoom.Entities;
using Overoom.Domain.Rooms.YoutubeRoom.Entities;
using Overoom.Domain.Users.Entities;

namespace Overoom.Application.Abstractions.StartPage.Interfaces;

public interface IStartPageMapper
{
    CommentStartPageDto MapComment(Comment comment, User? user);

    RoomStartPageDto MapFilmRoom(FilmRoom room, Film film);
    RoomStartPageDto MapYoutubeRoom(YoutubeRoom room);
    FilmStartPageDto MapFilm(Film film);
}