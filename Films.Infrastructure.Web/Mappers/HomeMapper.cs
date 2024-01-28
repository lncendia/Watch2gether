using Films.Application.Abstractions.StartPage.DTOs;
using Films.Infrastructure.Web.Mappers.Abstractions;
using Films.Infrastructure.Web.Models.Home;

namespace Films.Infrastructure.Web.Mappers;

public class HomeMapper : IHomeMapper
{
    public CommentStartPageViewModel Map(CommentDto dto) =>
        new(dto.Name, dto.Text, dto.DateTime, dto.FilmId, dto.AvatarUri);

    public RoomStartPageViewModel Map(RoomDto dto) => new(dto.Id, dto.Type, dto.CountUsers, dto.NowPlaying);

    public FilmStartPageViewModel Map(FilmDto dto) => new(dto.Name, dto.PosterUrl, dto.Id, dto.Genres);
}