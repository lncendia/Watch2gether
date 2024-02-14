using Room.Infrastructure.Storage.Models.Room.Base;

namespace Room.Infrastructure.Storage.Models.Room.YoutubeRoom;

public class YoutubeRoomModel : RoomModel
{
    public List<YoutubeViewerModel> Viewers { get; set; } = [];
    public List<VideoModel> Videos { get; set; } = [];
    
    public List<BannedModel<YoutubeRoomModel>> BannedUsers { get; set; } = [];
    
    public List<MessageModel<YoutubeRoomModel>> Messages { get; set; } = [];
    public bool Access { get; set; }
}