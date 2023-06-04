using Overoom.Infrastructure.Storage.Models.Rooms.Base;

namespace Overoom.Infrastructure.Storage.Models.Rooms;

public class FilmViewerModel : ViewerBaseModel
{
    public int Season { get; set; }
    public int Series { get; set; }
}