using Room.Application.Abstractions.DTOs.Rooms;
using Room.Domain.Rooms.YoutubeRooms.ValueObjects;

namespace Room.Application.Abstractions.DTOs.YoutubeRooms;

public class YoutubeRoomDto : RoomDto<YoutubeViewerDto>
{
    public required IReadOnlyCollection<Video> Videos { get; init; }
    public required bool VideoAccess { get; init; }
}