namespace Films.Application.Abstractions.Rooms.DTOs.Youtube;

public class YoutubeViewerDto : ViewerDto
{
    public required string CurrentVideoId { get; init; }
}