using Overoom.Application.Abstractions.Rooms.DTOs.Youtube;

namespace Overoom.Application.Abstractions.Rooms.Interfaces;

public interface IYoutubeRoomManager : IRoomManager
{
    Task<YoutubeRoomDto> GetAsync(Guid roomId);
    Task<(Guid roomId, YoutubeViewerDto viewer)> CreateAsync(string url, string name, bool addAccess);
    Task<(Guid roomId, YoutubeViewerDto viewer)> CreateForUserAsync(string url, Guid userId, bool addAccess);
    Task<YoutubeViewerDto> ConnectAsync(Guid roomId, int viewerId);
    Task<YoutubeViewerDto> ConnectAsync(Guid roomId, string name);
    Task<YoutubeViewerDto> ConnectForUserAsync(Guid roomId, Guid userId);
    Task<string> AddVideoAsync(Guid roomId, int viewerId, string url);
    Task RemoveVideoAsync(Guid roomId, string id);
    Task ChangeVideoAsync(Guid roomId, int viewerId, string id);
}