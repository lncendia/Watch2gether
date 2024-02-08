using Room.Application.Abstractions.DTOs.Base;

namespace Room.Application.Abstractions.DTOs.YoutubeRoom;

public class YoutubeRoomDto : RoomDto<YoutubeViewerDto>
{
    public required IReadOnlyCollection<string> Ids { get; init; }
    public required bool VideoAccess { get; init; }
}