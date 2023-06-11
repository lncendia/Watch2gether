using Overoom.Infrastructure.Storage.Models.Room.Base;

namespace Overoom.Infrastructure.Storage.Models.Room.YoutubeRoom;

public class YoutubeViewerModel : ViewerModel
{
    public string CurrentVideoId { get; set; } = null!;
}