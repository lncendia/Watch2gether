using Room.Application.Abstractions.Queries.DTOs.BaseRoom;
using Room.Domain.YoutubeRooms.ValueObjects;

namespace Room.Application.Abstractions.Queries.DTOs.YoutubeRoom;

public class YoutubeRoomDto : RoomDto<YoutubeViewerDto>
{
    public required IReadOnlyCollection<Video> Videos { get; init; }
    public required bool VideoAccess { get; init; }
}