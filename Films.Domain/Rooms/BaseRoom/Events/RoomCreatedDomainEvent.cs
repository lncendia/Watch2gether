using Films.Domain.Abstractions;
using Films.Domain.Users;

namespace Films.Domain.Rooms.BaseRoom.Events;

/// <summary>
/// Класс, представляющий событие создания новой комнаты.
/// </summary>
public abstract class RoomCreatedDomainEvent<T> : IDomainEvent where T : Room
{
    public required User Owner { get; init; }
    public required T Room { get; init; }
}