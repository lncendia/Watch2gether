using Room.Domain.Abstractions;

namespace Room.Domain.Rooms.FilmRooms.Events;

public class FilmRoomNoViewersDomainEvent(FilmRoom room) : IDomainEvent
{
    public FilmRoom Room { get; } = room;
}