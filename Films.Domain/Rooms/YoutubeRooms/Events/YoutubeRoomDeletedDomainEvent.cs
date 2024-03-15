using Films.Domain.Abstractions;

namespace Films.Domain.Rooms.YoutubeRooms.Events;

public class YoutubeRoomDeletedDomainEvent(YoutubeRoom room) : IDomainEvent
{
    public YoutubeRoom Room { get; } = room;
}