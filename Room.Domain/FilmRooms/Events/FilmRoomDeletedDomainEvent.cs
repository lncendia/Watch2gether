using Room.Domain.Abstractions;

namespace Room.Domain.FilmRooms.Events;

public class FilmRoomDeletedDomainEvent(FilmRoom room) : IDomainEvent
{
    public FilmRoom Room { get; } = room;
}