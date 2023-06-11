using Overoom.Domain.Film.Enums;
using Overoom.Infrastructure.Storage.Models.Room.Base;

namespace Overoom.Infrastructure.Storage.Models.Room.FilmRoom;

public class FilmRoomModel : RoomModel
{
    public Guid FilmId { get; set; }
    public CdnType CdnType { get; set; }
}