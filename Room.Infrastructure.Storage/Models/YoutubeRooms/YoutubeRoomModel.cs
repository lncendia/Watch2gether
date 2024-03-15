using Room.Infrastructure.Storage.Models.Rooms;

namespace Room.Infrastructure.Storage.Models.YoutubeRooms;

public class YoutubeRoomModel : RoomModel
{
    public List<YoutubeViewerModel> Viewers { get; set; } = [];
    public List<VideoModel> Videos { get; set; } = [];
    public bool VideoAccess { get; set; }
}