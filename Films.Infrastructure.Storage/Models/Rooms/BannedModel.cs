using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Storage.Models.Rooms;

[PrimaryKey(nameof(UserId), nameof(RoomId))]
public class BannedModel<TR> where TR : RoomModel
{
    public Guid UserId { get; set; }
    public Guid RoomId { get; set; }
    public TR Room { get; set; } = null!;
}