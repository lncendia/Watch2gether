using Room.Application.Abstractions.DTOs.Base;
using Room.Domain.Rooms.YoutubeRoom.ValueObjects;

namespace Room.Application.Abstractions.DTOs.YoutubeRoom;

public class YoutubeRoomDto : RoomDto<YoutubeViewerDto>
{
    public required IReadOnlyCollection<Video> Videos { get; init; }
    public required bool VideoAccess { get; init; }
}