using System.ComponentModel.DataAnnotations.Schema;
using Films.Domain.Rooms.Enums;
using Films.Infrastructure.Storage.Models.Abstractions;
using Films.Infrastructure.Storage.Models.Server;
using Films.Infrastructure.Storage.Models.User;

namespace Films.Infrastructure.Storage.Models.Room;

public class RoomModel : IAggregateModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    /// <summary> 
    /// Возвращает флаг открытости комнаты. 
    /// </summary>
    public bool IsOpen { get; set; }

    /// <summary> 
    /// Владелец комнаты. 
    /// </summary> 
    public Guid OwnerId { get; set; }

    public UserModel? Owner { get; set; }

    /// <summary> 
    /// Сервер.
    /// </summary> 
    public Guid ServerId { get; set; }

    public ServerModel? ServerModel { get; set; }

    /// <summary> 
    /// Тип комнаты.
    /// </summary> 
    public RoomType Type { get; set; }

    /// <summary> 
    /// Кол-во зрителей
    /// </summary> 
    public int ViewersCount { get; set; }
}