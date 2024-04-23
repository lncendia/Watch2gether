namespace Films.Application.Abstractions.DTOs.Rooms;

public class YoutubeRoomShortDto : RoomDto
{
    public required bool VideoAccess { get; init; }
}