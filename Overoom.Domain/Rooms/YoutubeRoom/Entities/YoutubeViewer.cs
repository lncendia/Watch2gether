using Overoom.Domain.Rooms.BaseRoom.Entities;

namespace Overoom.Domain.Rooms.YoutubeRoom.Entities;

public class YoutubeViewer : Viewer
{
    internal YoutubeViewer(int id, string name, string avatarUri, string currentVideoId) : base(id, name, avatarUri)
    {
        CurrentVideoId = currentVideoId;
    }
    public string CurrentVideoId { get; set; }
}