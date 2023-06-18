using Overoom.Application.Abstractions.Rooms.DTOs.Film;
using Overoom.Domain.Films.Enums;

namespace Overoom.Application.Abstractions.Rooms.Interfaces;

public interface IFilmRoomManager : IRoomManager<FilmRoomDto, FilmViewerDto>
{
    Task<(Guid roomId, FilmViewerDto viewer)> CreateAsync(Guid filmId, CdnType cdn, string name);
    Task<(Guid roomId, FilmViewerDto viewer)> CreateForUserAsync(Guid filmId, CdnType cdn, Guid userId);
    Task ChangeSeries(Guid roomId, int viewerId, int season, int series);
}