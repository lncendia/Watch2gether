using Overoom.Application.Abstractions.DTO.Rooms.Film;

namespace Overoom.Application.Abstractions.Interfaces.Rooms;

public interface IFilmRoomManager : IRoomManager
{
    Task<FilmRoomDto> GetAsync(Guid roomId);
    Task<(Guid roomId, FilmViewerDto viewer)> CreateAsync(Guid filmId, string name);
    Task<(Guid roomId, FilmViewerDto viewer)> CreateForUserAsync(Guid filmId, string email);
    Task ChangeSeries(Guid roomId, Guid viewerId, int season, int series);
    Task<FilmViewerDto> ConnectAsync(Guid roomId, Guid viewerId);
    Task<FilmViewerDto> ConnectAsync(Guid roomId, string name);
    Task<FilmViewerDto> ConnectForUserAsync(Guid roomId, string email);
}