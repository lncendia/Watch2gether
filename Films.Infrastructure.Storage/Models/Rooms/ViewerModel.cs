using Films.Infrastructure.Storage.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Storage.Models.Rooms;

[PrimaryKey(nameof(UserId), nameof(RoomId))]
public class ViewerModel<TR> where TR : RoomModel
{
    public Guid UserId { get; set; }
    public UserModel User { get; set; } = null!;
    
    public Guid RoomId { get; set; }
    public TR Room { get; set; }= null!;
}