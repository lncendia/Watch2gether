using Room.Domain.Users.Entities;
using Viewer = Room.Domain.Rooms.BaseRoom.Entities.Viewer;

namespace Room.Domain.Rooms.YoutubeRoom.Entities;

public class YoutubeViewer : Viewer
{
    internal YoutubeViewer(User user, int currentVideoNumber) : base(user) => CurrentVideoNumber = currentVideoNumber;

    public int CurrentVideoNumber { get; internal set; }
}