using Overoom.Infrastructure.Storage.Models.Rooms.Base;

namespace Overoom.Infrastructure.Storage.Models.Rooms.YoutubeRoom;

public class YoutubeViewerModel : ViewerModel
{
    public string CurrentVideoId { get; set; } = null!;
}