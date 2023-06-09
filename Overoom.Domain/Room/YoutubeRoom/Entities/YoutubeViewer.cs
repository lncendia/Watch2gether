using Overoom.Domain.Room.BaseRoom.Entities;

namespace Overoom.Domain.Room.YoutubeRoom.Entities;

public class YoutubeViewer : Viewer
{
    internal YoutubeViewer(int id, string name, string avatarUri, string currentVideoId) : base(id, name, avatarUri)
    {
        CurrentVideoId = currentVideoId;
    }
    public string CurrentVideoId { get; set; }
}