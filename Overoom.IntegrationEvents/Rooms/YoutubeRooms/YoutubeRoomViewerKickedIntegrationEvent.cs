namespace Overoom.IntegrationEvents.Rooms.YoutubeRooms;

public class YoutubeRoomViewerKickedIntegrationEvent
{
    public required Guid RoomId { get; init; }
    public required Guid ViewerId { get; init; }
}