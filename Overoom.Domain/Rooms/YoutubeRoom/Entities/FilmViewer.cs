using Overoom.Domain.Rooms.BaseRoom.Entities;

namespace Overoom.Domain.Rooms.YoutubeRoom.Entities;

public class YoutubeViewer : Viewer
{
    public YoutubeViewer(string name, Guid roomId, string avatarFileName, string currentVideoId) : base(name, roomId, avatarFileName)
    {
        CurrentVideoId = currentVideoId;
    }
    public string CurrentVideoId { get; set; }
}