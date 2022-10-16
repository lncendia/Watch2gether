using Watch2gether.Infrastructure.PersistentStorage.Models.Rooms.Base;

namespace Watch2gether.Infrastructure.PersistentStorage.Models.Rooms;

public class YoutubeRoomModel : RoomBaseModel
{
    public List<VideoIdModel> VideoIds { get; set; } = new();
    public List<YoutubeViewerModel> Viewers { get; set; } = new();
}