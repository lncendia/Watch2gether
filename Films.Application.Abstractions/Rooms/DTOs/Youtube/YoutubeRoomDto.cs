namespace Films.Application.Abstractions.Rooms.DTOs.Youtube;

public class YoutubeRoomDto : RoomDto
{
    public required IReadOnlyCollection<string> Ids { get; init; }
    public required bool Access { get; init; }
    public required IReadOnlyCollection<YoutubeViewerDto> Viewers { get; init; }
}