using Films.Domain.Abstractions;

namespace Films.Domain.Rooms.FilmRooms.Events;

public class FilmRoomDeletedDomainEvent(FilmRoom room) : IDomainEvent
{
    public FilmRoom Room { get; } = room;
}