using Room.Domain.Abstractions;

namespace Room.Domain.Rooms.YoutubeRooms.Events;

public class YoutubeRoomNoViewersDomainEvent(YoutubeRoom room) : IDomainEvent
{
    public YoutubeRoom Room { get; } = room;
}