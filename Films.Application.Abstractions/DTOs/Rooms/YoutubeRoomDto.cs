namespace Films.Application.Abstractions.DTOs.Rooms;

public class YoutubeRoomDto : YoutubeRoomShortDto
{
    public required bool IsCodeNeeded { get; init; }
}