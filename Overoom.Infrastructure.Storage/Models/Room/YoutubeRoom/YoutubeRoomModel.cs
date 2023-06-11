using Overoom.Infrastructure.Storage.Models.Room.Base;

namespace Overoom.Infrastructure.Storage.Models.Room.YoutubeRoom;

public class YoutubeRoomModel : RoomModel
{
    public List<VideoIdModel> VideoIds { get; set; } = new();
    public bool AddAccess { get; set; }
}