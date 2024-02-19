namespace Films.Application.Abstractions.Queries.Rooms.DTOs;

public class YoutubeRoomDto:RoomDto
{
    public required bool VideoAccess { get; init; }
}