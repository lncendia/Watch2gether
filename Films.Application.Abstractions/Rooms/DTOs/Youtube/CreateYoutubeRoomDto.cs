namespace Films.Application.Abstractions.Rooms.DTOs.Youtube;

public class CreateYoutubeRoomDto : CreateRoomDto
{
    public required Uri Url { get; init; }
    public required bool Access { get; init; }
}