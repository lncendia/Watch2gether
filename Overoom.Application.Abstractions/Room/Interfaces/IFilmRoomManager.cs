using Overoom.Application.Abstractions.Room.DTOs.Film;
using Overoom.Domain.Film.Enums;

namespace Overoom.Application.Abstractions.Room.Interfaces;

public interface IFilmRoomManager : IRoomManager<FilmRoomDto, FilmViewerDto>
{
    Task<(Guid roomId, FilmViewerDto viewer)> CreateAsync(Guid filmId, CdnType cdn, string name);
    Task<(Guid roomId, FilmViewerDto viewer)> CreateForUserAsync(Guid filmId, CdnType cdn, Guid userId);
    Task ChangeSeries(Guid roomId, int viewerId, int season, int series);
}