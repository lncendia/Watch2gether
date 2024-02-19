using Films.Infrastructure.Storage.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Storage.Models.Rooms.BaseRoom;

[PrimaryKey(nameof(UserId), nameof(RoomId))]
public class BannedModel<TR> where TR : RoomModel
{
    public Guid UserId { get; set; }
    public UserModel User { get; set; } = null!;

    public Guid RoomId { get; set; }
    public TR Room { get; set; } = null!;
}