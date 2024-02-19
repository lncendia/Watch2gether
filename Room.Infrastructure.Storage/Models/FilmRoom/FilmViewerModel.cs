using Room.Infrastructure.Storage.Models.BaseRoom;

namespace Room.Infrastructure.Storage.Models.FilmRoom;

public class FilmViewerModel : ViewerModel<FilmRoomModel>
{
    public int? Season { get; set; }

    public int? Series { get; set; }
}