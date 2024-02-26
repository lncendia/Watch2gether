using Overoom.IntegrationEvents.Rooms.BaseRooms;

namespace Overoom.IntegrationEvents.Rooms.FilmRooms;

public class FilmRoomViewerConnectedIntegrationEvent
{
    public required Guid RoomId { get; init; }
    public required Viewer Viewer { get; init; }
}