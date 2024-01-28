using Films.Application.Abstractions.Rooms.DTOs;
using Films.Application.Abstractions.Rooms.DTOs.Film;
using Films.Infrastructure.Web.Mappers.Abstractions;
using Films.Infrastructure.Web.Models.Rooms;
using Films.Infrastructure.Web.Models.Rooms.FilmRoom;

namespace Films.Infrastructure.Web.Mappers;

public class FilmRoomMapper : IFilmRoomMapper
{
    public FilmRoomViewModel Map(FilmRoomDto dto, int id, string url)
    {
        var messages = dto.Messages.Select(Map).ToList();
        var viewers = dto.Viewers.Select(Map).ToList();
        var film = new FilmViewModel(dto.Film.Name, dto.Film.Url, dto.Film.Type, dto.Film.Cdn);
        return new FilmRoomViewModel(messages, viewers, film, url, dto.OwnerId, id, dto.IsOpen);
    }

    private static FilmViewerViewModel Map(FilmViewerDto dto) =>
        new(dto.Id, dto.Username, dto.AvatarUrl, dto.Pause, dto.Time, dto.Season, dto.Series, dto.FullScreen,
            dto.Allows.Beep, dto.Allows.Scream, dto.Allows.Change);

    private static MessageViewModel Map(MessageDto dto) =>
        new(dto.Text, dto.CreatedAt, dto.ViewerId, dto.AvatarUri, dto.Username);
}