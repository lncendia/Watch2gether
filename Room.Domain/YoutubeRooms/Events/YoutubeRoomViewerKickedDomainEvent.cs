using Room.Domain.Abstractions;

namespace Room.Domain.YoutubeRooms.Events;

public class YoutubeRoomViewerKickedDomainEvent(YoutubeRoom room, Guid viewerId) : IDomainEvent
{
    public YoutubeRoom Room { get; } = room;
    public Guid ViewerId { get; } = viewerId;
}