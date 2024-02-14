using Room.Application.Abstractions.DTOs.Base;

namespace Room.Application.Abstractions.DTOs.YoutubeRoom;

public class YoutubeViewerDto : ViewerDto
{
    /// <summary>
    /// Идентификатор видео
    /// </summary>
    public required string VideoId { get; init; }
}