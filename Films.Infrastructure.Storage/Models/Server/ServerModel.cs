using System.ComponentModel.DataAnnotations.Schema;
using Films.Infrastructure.Storage.Models.Abstractions;

namespace Films.Infrastructure.Storage.Models.Server;

public class ServerModel : IAggregateModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    public int MaxRoomsCount { get; set; }
    public int RoomsCount { get; set; }
    public bool IsEnabled { get; set; }
    public Uri Url { get; set; } = null!;
}