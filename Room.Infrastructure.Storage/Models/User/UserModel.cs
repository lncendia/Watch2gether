using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Room.Infrastructure.Storage.Models.Abstractions;

namespace Room.Infrastructure.Storage.Models.User;

public class UserModel : IAggregateModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    [MaxLength(20)] public string UserName { get; set; } = null!;
    public Uri PhotoUrl { get; set; } = null!;
    public bool Beep { get; set; }
    public bool Scream { get; set; }
    public bool Change { get; set; }
}