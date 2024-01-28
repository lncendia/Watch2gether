using Films.Domain.Abstractions;
using Films.Domain.Rooms.Enums;

namespace Films.Domain.Rooms.Entities;

/// <summary> 
/// Абстрактный класс, представляющий базовую комнату. 
/// </summary> 
public class Room : AggregateRoot
{
    /// <summary> 
    /// Возвращает флаг открытости комнаты. 
    /// </summary> 
    public required bool IsOpen { get; init; }

    /// <summary> 
    /// Владелец комнаты. 
    /// </summary> 
    public required Guid OwnerId { get; init; }

    /// <summary> 
    /// Сервер.
    /// </summary> 
    public required Guid ServerId { get; init; }

    /// <summary> 
    /// Тип комнаты.
    /// </summary> 
    public required RoomType Type { get; init; }
    
    /// <summary> 
    /// Кол-во зрителей
    /// </summary> 
    public int ViewersCount { get; set; }
}