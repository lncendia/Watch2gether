using Room.Domain.Abstractions;

namespace Room.Domain.Rooms.YoutubeRooms.Events;

public class YoutubeRoomViewerLeavedDomainEvent(YoutubeRoom room, Guid viewerId) : IDomainEvent
{
    public YoutubeRoom Room { get; } = room;
    public Guid ViewerId { get; } = viewerId;
}