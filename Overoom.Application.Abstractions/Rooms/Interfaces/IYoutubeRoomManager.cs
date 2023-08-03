using Overoom.Application.Abstractions.Rooms.DTOs.Youtube;

namespace Overoom.Application.Abstractions.Rooms.Interfaces;

public interface IYoutubeRoomManager : IRoomManager
{
    Task<YoutubeRoomDto> GetAsync(Guid roomId);
    Task<YoutubeViewerDto> GetAsync(Guid roomId, int viewerId);
    Task<(Guid roomId, int viewerId)> CreateAnonymouslyAsync(CreateYoutubeRoomDto dto, string name);
    Task<(Guid roomId, int viewerId)> CreateAsync(CreateYoutubeRoomDto dto, Guid userId);
    Task<string> AddVideoAsync(Guid roomId, int viewerId, Uri url);
    Task RemoveVideoAsync(Guid roomId, string id);
    Task ChangeVideoAsync(Guid roomId, int viewerId, string id);
}