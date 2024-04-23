using Room.Infrastructure.Web.Rooms.ViewModels.Common;

namespace Room.Infrastructure.Web.Rooms.ViewModels.YoutubeRooms;

public class YoutubeRoomViewModel:RoomViewModel<YoutubeViewerViewModel>
{
    public required IReadOnlyCollection<string> Videos { get; init; }
    public required bool VideoAccess { get; init; }
}