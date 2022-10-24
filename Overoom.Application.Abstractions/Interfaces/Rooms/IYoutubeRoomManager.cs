using Overoom.Application.Abstractions.DTO.Rooms.Youtube;

namespace Overoom.Application.Abstractions.Interfaces.Rooms;

public interface IYoutubeRoomManager : IRoomManager
{
    Task<YoutubeRoomDto> GetAsync(Guid roomId);
    Task<(Guid roomId, YoutubeViewerDto viewer)> CreateAsync(string url, string name, bool addAccess);
    Task<(Guid roomId, YoutubeViewerDto viewer)> CreateForUserAsync(string url, string email, bool addAccess);
    Task<string> AddVideoAsync(Guid roomId, Guid viewerId, string url);
    Task RemoveVideoAsync(Guid roomId, string id);
    Task ChangeVideoAsync(Guid roomId, Guid viewerId, string id);
    Task<YoutubeViewerDto> ConnectAsync(Guid roomId, Guid viewerId);
    Task<YoutubeViewerDto> ConnectAsync(Guid roomId, string name);
    Task<YoutubeViewerDto> ConnectForUserAsync(Guid roomId, string email);
}