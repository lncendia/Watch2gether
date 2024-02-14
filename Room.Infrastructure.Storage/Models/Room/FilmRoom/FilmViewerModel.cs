using Room.Infrastructure.Storage.Models.Room.Base;

namespace Room.Infrastructure.Storage.Models.Room.FilmRoom;

public class FilmViewerModel : ViewerModel<FilmRoomModel>
{
    public int? Season { get; set; }

    public int? Series { get; set; }
}