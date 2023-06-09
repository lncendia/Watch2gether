using Overoom.Infrastructure.Storage.Models.Rooms.Base;

namespace Overoom.Infrastructure.Storage.Models.Rooms.FilmRoom;

public class FilmViewerModel : ViewerModel
{
    public int Season { get; set; }
    public int Series { get; set; }
}