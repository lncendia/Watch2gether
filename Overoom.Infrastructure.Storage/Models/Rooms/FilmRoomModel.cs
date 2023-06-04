using Overoom.Infrastructure.Storage.Models.Rooms.Base;

namespace Overoom.Infrastructure.Storage.Models.Rooms;

public class FilmRoomModel : RoomBaseModel
{
    public Guid FilmId { get; set; }
    public List<FilmViewerModel> Viewers { get; set; } = new();
}