using Overoom.Infrastructure.PersistentStorage.Models.Rooms.Base;

namespace Overoom.Infrastructure.PersistentStorage.Models.Rooms;

public class FilmViewerModel : ViewerBaseModel
{
    public int Season { get; set; }
    public int Series { get; set; }
}