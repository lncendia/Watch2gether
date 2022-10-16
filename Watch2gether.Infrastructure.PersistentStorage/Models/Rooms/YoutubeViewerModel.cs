using Watch2gether.Infrastructure.PersistentStorage.Models.Rooms.Base;

namespace Watch2gether.Infrastructure.PersistentStorage.Models.Rooms;

public class YoutubeViewerModel : ViewerBaseModel
{
    public string CurrentVideoId { get; set; } = null!;
}