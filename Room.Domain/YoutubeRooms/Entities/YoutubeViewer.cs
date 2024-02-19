using Viewer = Room.Domain.BaseRoom.Entities.Viewer;

namespace Room.Domain.YoutubeRooms.Entities;

public class YoutubeViewer : Viewer
{
    public string? VideoId { get; internal set; }
}