using Room.Domain.Rooms.Rooms.Entities;

namespace Room.Domain.Rooms.YoutubeRooms.Entities;

public class YoutubeViewer : Viewer
{
    public string? VideoId { get; internal set; }
}