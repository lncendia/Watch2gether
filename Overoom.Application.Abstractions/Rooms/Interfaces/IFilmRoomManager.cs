using Overoom.Application.Abstractions.Rooms.DTOs;
using Overoom.Application.Abstractions.Rooms.DTOs.Film;
using Overoom.Domain.Films.Enums;

namespace Overoom.Application.Abstractions.Rooms.Interfaces;

public interface IFilmRoomManager : IRoomManager
{
    Task<FilmRoomDto> GetAsync(Guid roomId);
    Task<(Guid roomId, FilmViewerDto viewer)> CreateAsync(Guid filmId, CdnType cdn, string name);
    Task<(Guid roomId, FilmViewerDto viewer)> CreateForUserAsync(Guid filmId, CdnType cdn, Guid userId);
    Task<FilmViewerDto> ConnectAsync(Guid roomId, int viewerId);
    Task<FilmViewerDto> ConnectAsync(Guid roomId, string name);
    Task<FilmViewerDto> ConnectForUserAsync(Guid roomId, Guid userId);
    Task ChangeSeries(Guid roomId, int viewerId, int season, int series);
}