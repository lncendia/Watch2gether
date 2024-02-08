using Room.Application.Abstractions.DTOs.Base;

namespace Room.Application.Abstractions.DTOs.YoutubeRoom;

public class YoutubeViewerDto : ViewerDto
{
    public required string CurrentVideoId { get; init; }
}