using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Room.Infrastructure.Storage.Models.Abstractions;
using Room.Infrastructure.Storage.Models.Rooms;

namespace Room.Infrastructure.Storage.Models.Messages;

public class MessageModel<TR> : IAggregateModel where TR : RoomModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid RoomId { get; set; }

    public TR Room { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    [MaxLength(1000)] public string Text { get; set; } = null!;
}