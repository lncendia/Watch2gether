using Overoom.Infrastructure.PersistentStorage.Models.Rooms.Base;

namespace Overoom.Infrastructure.PersistentStorage.Models.Rooms;

public class YoutubeViewerModel : ViewerBaseModel
{
    public string CurrentVideoId { get; set; } = null!;
}