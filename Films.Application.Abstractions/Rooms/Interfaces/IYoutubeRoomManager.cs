using Films.Application.Abstractions.Rooms.DTOs.Youtube;

namespace Films.Application.Abstractions.Rooms.Interfaces;

public interface IYoutubeRoomManager : IRoomManager
{
    Task<YoutubeRoomDto> GetAsync(Guid roomId);
    Task<YoutubeViewerDto> GetAsync(Guid roomId, int viewerId);
    Task<(Guid roomId, int viewerId)> CreateAnonymouslyAsync(CreateYoutubeRoomDto dto, string name);
    Task<(Guid roomId, int viewerId)> CreateAsync(CreateYoutubeRoomDto dto, Guid userId);
    Task<string> AddVideoAsync(Guid roomId, int viewerId, Uri url);
    Task ChangeVideoAsync(Guid roomId, int viewerId, string id);
}