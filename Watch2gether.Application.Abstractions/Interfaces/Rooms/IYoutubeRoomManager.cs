using Watch2gether.Application.Abstractions.DTO.Rooms;

namespace Watch2gether.Application.Abstractions.Interfaces.Rooms;

public interface IYoutubeRoomManager : IRoomManager
{
    Task<YoutubeRoomDto> GetAsync(Guid roomId, Guid viewerId);
    Task<(Guid roomId, ViewerDto viewer)> CreateAsync(string ling, string name);
    Task<(Guid roomId, ViewerDto viewer)> CreateForUserAsync(string link, string email);
}