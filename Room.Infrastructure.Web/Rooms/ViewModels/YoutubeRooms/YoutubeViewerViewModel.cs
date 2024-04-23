using Room.Infrastructure.Web.Rooms.ViewModels.Common;

namespace Room.Infrastructure.Web.Rooms.ViewModels.YoutubeRooms;

public class YoutubeViewerViewModel : ViewerViewModel
{
    public string? VideoId { get; init; }
}