using Overoom.Infrastructure.PersistentStorage.Models.Rooms.Base;

namespace Overoom.Infrastructure.PersistentStorage.Models.Rooms;

public class FilmRoomModel : RoomBaseModel
{
    public Guid FilmId { get; set; }
    public List<FilmViewerModel> Viewers { get; set; } = new();
}