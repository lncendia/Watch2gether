namespace Overoom.IntegrationEvents.Rooms.YoutubeRooms;

public class YoutubeRoomViewerLeavedIntegrationEvent
{
    public required Guid RoomId { get; init; }
    public required Guid ViewerId { get; init; }
}