using Films.Domain.Abstractions;
using Films.Domain.Users;

namespace Films.Domain.Rooms.YoutubeRooms.Events;

public class YoutubeRoomUserConnectedDomainEvent(YoutubeRoom room, User user) : IDomainEvent
{
    public YoutubeRoom Room { get; } = room;
    public User User { get; } = user;
}