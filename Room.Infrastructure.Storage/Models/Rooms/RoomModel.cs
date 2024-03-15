using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Room.Infrastructure.Storage.Models.Abstractions;

namespace Room.Infrastructure.Storage.Models.Rooms;

public abstract class RoomModel : IAggregateModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    
}