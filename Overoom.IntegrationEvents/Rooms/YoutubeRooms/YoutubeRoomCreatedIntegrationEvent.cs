using Overoom.IntegrationEvents.Rooms.BaseRooms;

namespace Overoom.IntegrationEvents.Rooms.YoutubeRooms;

public class YoutubeRoomCreatedIntegrationEvent
{
    public required Guid Id { get; init; }
    
    public required bool VideoAccess { get; init; }
    
    public required Viewer Owner { get; init; }
}