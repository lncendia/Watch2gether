using System.ComponentModel.DataAnnotations.Schema;
using Films.Infrastructure.Storage.Models.Abstractions;
using Films.Infrastructure.Storage.Models.Servers;

namespace Films.Infrastructure.Storage.Models.Rooms;

public class RoomModel : IAggregateModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    /// <summary> 
    /// Секретный код комнаты
    /// </summary>
    public string? Code { get; set; }

    /// <summary> 
    /// Идентификатор сервера.
    /// </summary> 
    public Guid ServerId { get; set; }

    /// <summary>
    /// Сервер.
    /// </summary>
    public ServerModel Server { get; set; } = null!;
}