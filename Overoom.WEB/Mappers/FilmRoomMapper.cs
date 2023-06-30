using Overoom.Application.Abstractions.Rooms.DTOs.Film;
using Overoom.WEB.Mappers.Abstractions;
using Overoom.WEB.Models.Rooms.FilmRoom;

namespace Overoom.WEB.Mappers;

public class FilmRoomMapper : IFilmRoomMapper
{
    public FilmRoomViewModel Map(FilmRoomDto dto, int id, string url)
    {
        var messages = dto.Messages.Select(Map);
        var viewers = dto.Viewers.Select(Map);
        var film = new FilmViewModel(dto.Film.Name, dto.Film.Url, dto.Film.Type);
        return new FilmRoomViewModel(messages, viewers, film, url, dto.OwnerId, id);
    }
}