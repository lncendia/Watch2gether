using Watch2gether.Application.Abstractions.DTO.Rooms;

namespace Watch2gether.Application.Abstractions.Interfaces.Rooms;

public interface IYoutubeRoomManager : IRoomManager
{
    Task<YoutubeRoomDto> GetAsync(Guid roomId, Guid viewerId);
    Task<(Guid roomId, ViewerDto viewer)> CreateAsync(string url, string name);
    Task<(Guid roomId, ViewerDto viewer)> CreateForUserAsync(string url, string email);
    Task<string> AddVideoAsync(Guid roomId, string url);
    Task RemoveVideoAsync(Guid roomId, string id);
}