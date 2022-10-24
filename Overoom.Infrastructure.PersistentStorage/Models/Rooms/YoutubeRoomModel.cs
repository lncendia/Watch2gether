using Overoom.Infrastructure.PersistentStorage.Models.Rooms.Base;

namespace Overoom.Infrastructure.PersistentStorage.Models.Rooms;

public class YoutubeRoomModel : RoomBaseModel
{
    public List<VideoIdModel> VideoIds { get; set; } = new();
    public List<YoutubeViewerModel> Viewers { get; set; } = new();
    public bool AddAccess { get; set; }
}