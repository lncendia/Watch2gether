using Films.Domain.Abstractions;

namespace Films.Domain.Rooms.YoutubeRooms.Events;

public class YoutubeRoomUserLeavedDomainEvent(YoutubeRoom room, Guid viewerId) : IDomainEvent
{
    public YoutubeRoom Room { get; } = room;
    public Guid UserId { get; } = viewerId;
}