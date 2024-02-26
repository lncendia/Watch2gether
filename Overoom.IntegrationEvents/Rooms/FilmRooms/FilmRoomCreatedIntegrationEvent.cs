using Overoom.IntegrationEvents.Rooms.BaseRooms;

namespace Overoom.IntegrationEvents.Rooms.FilmRooms;

public class FilmRoomCreatedIntegrationEvent
{
    public required Guid Id { get; init; }

    public required string Title { get; init; }

    public required Uri CdnUrl { get; init; }

    public required bool IsSerial { get; init; }

    public required Viewer Owner { get; init; }
}