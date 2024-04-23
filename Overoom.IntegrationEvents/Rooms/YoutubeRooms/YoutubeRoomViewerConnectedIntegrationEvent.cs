using Overoom.IntegrationEvents.Rooms.BaseRooms;

namespace Overoom.IntegrationEvents.Rooms.YoutubeRooms;

public class YoutubeRoomViewerConnectedIntegrationEvent
{
    public required Guid RoomId { get; init; }
    
    public required Guid ServerId { get; init; }
    public required Viewer Viewer { get; init; }
}