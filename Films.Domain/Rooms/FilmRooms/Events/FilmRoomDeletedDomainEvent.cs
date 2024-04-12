using Films.Domain.Abstractions;

namespace Films.Domain.Rooms.FilmRooms.Events;

public class FilmRoomDeletedDomainEvent : IDomainEvent
{
    public required FilmRoom FilmRoom { get; init; }
}