using Room.Application.Abstractions.Queries.DTOs.BaseRoom;

namespace Room.Application.Abstractions.Queries.DTOs.YoutubeRoom;

public class YoutubeViewerDto : ViewerDto
{
    /// <summary>
    /// Идентификатор видео
    /// </summary>
    public string? VideoId { get; init; }
}