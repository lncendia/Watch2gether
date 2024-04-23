using Room.Infrastructure.Web.Rooms.ViewModels.Common;

namespace Room.Infrastructure.Web.Rooms.ViewModels.FilmRooms;

public class FilmViewerViewModel : ViewerViewModel
{
    public int? Season { get; init; }
    public int? Series { get; init; }
}