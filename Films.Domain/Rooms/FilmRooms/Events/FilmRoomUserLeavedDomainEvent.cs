using Films.Domain.Abstractions;

namespace Films.Domain.Rooms.FilmRooms.Events;

public class FilmRoomUserLeavedDomainEvent(FilmRoom room, Guid viewerId) : IDomainEvent
{
    public FilmRoom Room { get; } = room;
    public Guid UserId { get; } = viewerId;
}