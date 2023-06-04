using Overoom.Domain.Room.BaseRoom.Entities;

namespace Overoom.Domain.Room.YoutubeRoom.Entities;

public class YoutubeViewer : Viewer
{
    public YoutubeViewer(int id, string name, string avatarFileName, string currentVideoId) : base(id, name, avatarFileName)
    {
        CurrentVideoId = currentVideoId;
    }
    public string CurrentVideoId { get; set; }
}