using Room.Domain.Abstractions;

namespace Room.Domain.YoutubeRooms.Events;

public class YoutubeRoomDeletedDomainEvent(YoutubeRoom room) : IDomainEvent
{
    public YoutubeRoom Room { get; } = room;
}