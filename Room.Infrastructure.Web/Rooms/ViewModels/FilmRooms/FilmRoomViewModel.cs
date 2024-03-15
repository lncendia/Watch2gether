using Room.Infrastructure.Web.Rooms.ViewModels.Common;

namespace Room.Infrastructure.Web.Rooms.ViewModels.FilmRooms;

public class FilmRoomViewModel:RoomViewModel<FilmViewerViewModel>
{
    public required string Title { get; init; }
    public required string CdnName { get; init; }
    public required string CdnUrl { get; init; }
    public required bool IsSerial { get; init; }
}