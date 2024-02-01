using Films.Application.Abstractions.Rooms.DTOs;
using Films.Application.Abstractions.Rooms.DTOs.Film;
using Films.Application.Abstractions.Rooms.Interfaces;
using Films.Domain.Films.Entities;

namespace Films.Application.Services.Rooms;

public class FilmRoomMapper : IFilmRoomMapper
{
    public FilmRoomDto Map(FilmRoom room, Film film)
    {
        
        var viewersEntities = room.Viewers;
        var viewers = viewersEntities.Where(x => x.Online).Select(Map).ToList();
        var messages = room.Messages
            .Select(m =>
            {
                var viewer = viewersEntities.First(x => x.Id == m.ViewerId);
                return new MessageDto(m.Text, m.CreatedAt, viewer.Id, viewer.AvatarUrl, viewer.Name);
            }).ToList();

        var filmDto = new FilmDataDto(film.Id, film.Title, film.CdnList.First(x => x.Type == room.CdnType).Uri,
            film.Type, room.CdnType);

        return new FilmRoomDto(filmDto, messages, viewers, room.Owner.Id, room.IsOpen);
    }

    public FilmViewerDto Map(FilmViewer v)
    {
        var allows = new AllowsDto(v.Allows.Beep, v.Allows.Scream, v.Allows.Change);
        return new FilmViewerDto(v.Name, v.Id, v.AvatarUrl, v.TimeLine, v.Pause, v.FullScreen, v.Season, v.Series, allows);
    }
}