using Films.Domain.Abstractions;
using Films.Domain.Users;

namespace Films.Domain.Rooms.FilmRooms.Events;

public class FilmRoomUserConnectedDomainEvent(FilmRoom room, User user) : IDomainEvent
{
    public FilmRoom Room { get; } = room;
    public User User { get; } = user;
}