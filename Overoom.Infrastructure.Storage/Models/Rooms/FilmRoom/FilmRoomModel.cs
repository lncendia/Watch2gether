using Overoom.Domain.Film.Enums;
using Overoom.Infrastructure.Storage.Models.Rooms.Base;

namespace Overoom.Infrastructure.Storage.Models.Rooms.FilmRoom;

public class FilmRoomModel : RoomModel
{
    public Guid FilmId { get; set; }
    public CdnType CdnType { get; set; }
}