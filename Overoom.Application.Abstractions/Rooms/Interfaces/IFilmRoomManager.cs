using Overoom.Application.Abstractions.Rooms.DTOs.Film;

namespace Overoom.Application.Abstractions.Rooms.Interfaces;

public interface IFilmRoomManager : IRoomManager
{
    Task<FilmRoomDto> GetAsync(Guid roomId);
    Task<FilmViewerDto> GetAsync(Guid roomId, int viewerId);
    Task<(Guid roomId, int viewerId)> CreateAnonymouslyAsync(CreateFilmRoomDto createFilmRoomDto, string name);
    Task<(Guid roomId, int viewerId)> CreateAsync(CreateFilmRoomDto createFilmRoomDto, Guid userId);
    Task ChangeSeries(Guid roomId, int viewerId, int season, int series);
}