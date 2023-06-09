using Overoom.Infrastructure.Storage.Models.Rooms.Base;

namespace Overoom.Infrastructure.Storage.Models.Rooms.YoutubeRoom;

public class YoutubeRoomModel : RoomModel
{
    public List<VideoIdModel> VideoIds { get; set; } = new();
    public bool AddAccess { get; set; }
}