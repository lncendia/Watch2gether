using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Room.Infrastructure.Storage.Models.BaseRoom;

[PrimaryKey(nameof(ViewerId), nameof(RoomId))]
public class MessageModel<TR> where TR : RoomModel
{
    public Guid ViewerId { get; set; }

    public Guid RoomId { get; set; }
    
    public TR Room { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    [MaxLength(1000)] public string Text { get; set; } = null!;
}