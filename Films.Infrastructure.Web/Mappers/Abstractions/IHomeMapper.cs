using Films.Application.Abstractions.StartPage.DTOs;
using Films.Infrastructure.Web.Models.Home;

namespace Films.Infrastructure.Web.Mappers.Abstractions;

public interface IHomeMapper
{
    CommentStartPageViewModel Map(CommentDto dto);
    RoomStartPageViewModel Map(RoomDto dto);
    FilmStartPageViewModel Map(FilmDto dto);
}