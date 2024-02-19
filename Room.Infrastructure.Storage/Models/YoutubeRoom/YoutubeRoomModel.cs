using Room.Infrastructure.Storage.Models.BaseRoom;

namespace Room.Infrastructure.Storage.Models.YoutubeRoom;

public class YoutubeRoomModel : RoomModel
{
    public List<YoutubeViewerModel> Viewers { get; set; } = [];
    public List<VideoModel> Videos { get; set; } = [];
    
    public List<MessageModel<YoutubeRoomModel>> Messages { get; set; } = [];
    public bool VideoAccess { get; set; }
}