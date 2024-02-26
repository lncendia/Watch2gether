namespace Overoom.IntegrationEvents.Rooms.FilmRooms;

public class FilmRoomViewerLeavedIntegrationEvent
{
    public required Guid RoomId { get; init; }
    public required Guid ViewerId { get; init; }
}