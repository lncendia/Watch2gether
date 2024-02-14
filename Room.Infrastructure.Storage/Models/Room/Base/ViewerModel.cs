using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Room.Infrastructure.Storage.Models.User;

namespace Room.Infrastructure.Storage.Models.Room.Base;

[PrimaryKey(nameof(UserId), nameof(RoomId))]
public abstract class ViewerModel<TR> where TR : RoomModel
{
    public Guid UserId { get; set; }

    public Guid RoomId { get; set; }
    
    public UserModel User { get; set; } = null!;
    public TR Room { get; set; } = null!;

    public bool Online { get; set; }
    public bool FullScreen { get; set; }
    public bool Pause { get; set; }
    public bool Owner { get; set; }
    public TimeSpan TimeLine { get; set; }
    [MaxLength(20)] public string? CustomName { get; set; }
}