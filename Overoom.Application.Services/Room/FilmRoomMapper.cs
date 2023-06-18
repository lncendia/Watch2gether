using Overoom.Application.Abstractions.Rooms.DTOs.Film;
using Overoom.Application.Abstractions.Rooms.Interfaces;
using Overoom.Domain.Rooms.FilmRoom.Entities;

namespace Overoom.Application.Services.Room;

public class FilmRoomMapper : IFilmRoomMapper
{
    public FilmRoomDto Map(FilmRoom room, Domain.Films.Entities.Film film)
    {
        var viewers = room.Viewers.Where(x => x.Online).Select(Map).ToList();

        var messages = room.Messages
            .Select(m => new FilmMessageDto(m.Text, m.CreatedAt, viewers.First(x => x.Id == m.ViewerId))).ToList();

        var filmDto = new FilmDataDto(film.Id, film.Name, film.CdnList.First(x => x.Type == room.CdnType).Uri,
            film.Type);

        return new FilmRoomDto(filmDto, messages, viewers, room.Owner.Id);
    }

    public FilmViewerDto Map(FilmViewer v) =>
        new(v.Name, v.Id, v.AvatarUri, v.TimeLine, v.OnPause, v.Season, v.Series);
}