using Watch2gether.Application.Abstractions.DTO.Rooms;
using Watch2gether.Domain.Rooms.YoutubeRoom;

namespace Watch2gether.Application.Abstractions.Interfaces.Rooms;

public interface IFilmRoomManager : IRoomManager
{
    Task<FilmRoomDto> GetAsync(Guid roomId, Guid viewerId);
    Task<(Guid roomId, ViewerDto viewer)> CreateAsync(Guid filmId, string name);
    Task<(Guid roomId, ViewerDto viewer)> CreateForUserAsync(Guid filmId, string email);
    Task ChangeSeason(Guid roomId, Guid viewerId, int season);
    Task ChangeSeries(Guid roomId, Guid viewerId, int series);
}