namespace Overoom.IntegrationEvents.Rooms;

public class FilmRoomCreatedIntegrationEvent
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }

    public required Uri CdnUrl { get; init; }

    public required bool IsSerial { get; init; }
}