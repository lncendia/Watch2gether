using Room.Domain.Users.Entities;
using Viewer = Room.Domain.Rooms.BaseRoom.Entities.Viewer;

namespace Room.Domain.Rooms.YoutubeRoom.Entities;

public class YoutubeViewer : Viewer
{
    internal YoutubeViewer(User user, string videoId) : base(user) => VideoId = videoId;

    public string VideoId { get; internal set; }
}