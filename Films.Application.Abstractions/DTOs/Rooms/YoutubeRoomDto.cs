namespace Films.Application.Abstractions.DTOs.Rooms;

public class YoutubeRoomDto:RoomDto
{
    public required bool VideoAccess { get; init; }
}