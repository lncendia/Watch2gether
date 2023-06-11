using Overoom.Infrastructure.Storage.Models.Room.Base;

namespace Overoom.Infrastructure.Storage.Models.Room.FilmRoom;

public class FilmViewerModel : ViewerModel
{
    public int Season { get; set; }
    public int Series { get; set; }
}