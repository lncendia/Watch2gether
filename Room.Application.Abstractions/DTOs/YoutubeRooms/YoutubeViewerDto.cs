using Room.Application.Abstractions.DTOs.Rooms;

namespace Room.Application.Abstractions.DTOs.YoutubeRooms;

public class YoutubeViewerDto : ViewerDto
{
    /// <summary>
    /// Идентификатор видео
    /// </summary>
    public string? VideoId { get; init; }
}