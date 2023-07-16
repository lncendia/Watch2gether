using Overoom.Application.Abstractions.StartPage.DTOs;
using Overoom.WEB.Models.Home;

namespace Overoom.WEB.Mappers.Abstractions;

public interface IHomeMapper
{
    CommentStartPageViewModel Map(CommentDto dto);
    RoomStartPageViewModel Map(RoomDto dto);
    FilmStartPageViewModel Map(FilmDto dto);
}