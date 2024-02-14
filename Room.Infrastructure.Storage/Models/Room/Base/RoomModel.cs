using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Room.Infrastructure.Storage.Models.Abstractions;

namespace Room.Infrastructure.Storage.Models.Room.Base;

public abstract class RoomModel : IAggregateModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public DateTime LastActivity { get; set; }

    [MaxLength(5)] public string? Code { get; set; }
}