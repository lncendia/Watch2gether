using Overoom.Domain.Rooms.BaseRoom.DTOs;
using Overoom.Domain.Rooms.BaseRoom.Entities;

namespace Overoom.Domain.Rooms.YoutubeRoom.Entities;

public class YoutubeViewer : Viewer
{
    internal YoutubeViewer(ViewerDto viewer, int id, string currentVideoId) : base(id, viewer)
    {
        CurrentVideoId = currentVideoId;
    }

    public string CurrentVideoId { get; set; }
}