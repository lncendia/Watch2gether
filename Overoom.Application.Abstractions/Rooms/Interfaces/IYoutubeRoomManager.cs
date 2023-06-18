using Overoom.Application.Abstractions.Rooms.DTOs.Youtube;

namespace Overoom.Application.Abstractions.Rooms.Interfaces;

public interface IYoutubeRoomManager : IRoomManager<YoutubeRoomDto, YoutubeViewerDto>
{
    Task<(Guid roomId, YoutubeViewerDto viewer)> CreateAsync(string url, string name, bool addAccess);
    Task<(Guid roomId, YoutubeViewerDto viewer)> CreateForUserAsync(string url, Guid userId, bool addAccess);
    Task<string> AddVideoAsync(Guid roomId, int viewerId, string url);
    Task RemoveVideoAsync(Guid roomId, string id);
    Task ChangeVideoAsync(Guid roomId, int viewerId, string id);
}