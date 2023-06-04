using Overoom.Infrastructure.Storage.Models.Rooms.Base;

namespace Overoom.Infrastructure.Storage.Models.Rooms;

public class YoutubeViewerModel : ViewerBaseModel
{
    public string CurrentVideoId { get; set; } = null!;
}