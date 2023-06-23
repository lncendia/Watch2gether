using Overoom.Application.Abstractions.StartPage.DTOs;
using Overoom.WEB.Models.Home;

namespace Overoom.WEB.Mappers.Abstractions;

public interface IHomeMapper
{
    CommentStartPageViewModel Map(CommentStartPageDto dto);
    RoomStartPageViewModel Map(RoomStartPageDto dto);
    FilmStartPageViewModel Map(FilmStartPageDto dto);
}