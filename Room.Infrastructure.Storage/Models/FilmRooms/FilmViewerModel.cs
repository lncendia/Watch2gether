using Room.Infrastructure.Storage.Models.Rooms;

namespace Room.Infrastructure.Storage.Models.FilmRooms;

public class FilmViewerModel : ViewerModel<FilmRoomModel>
{
    public int? Season { get; set; }

    public int? Series { get; set; }
}