using Films.Domain.Abstractions;
using Films.Domain.Servers.Events;

namespace Films.Domain.Servers.Entities;

/// <summary>
/// Класс, представляющий сервер.
/// </summary>
public class Server : AggregateRoot
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Server"/>.
    /// </summary>
    public Server()
    {
        AddDomainEvent(new ServerCreatedEvent
        {
            Id = Id
        });
    }

    /// <summary>
    /// Максимальное количество комнат, доступное на сервере.
    /// </summary>
    public required int MaxRoomsCount { get; set; }

    /// <summary>
    /// Текущее количество комнат на сервере.
    /// </summary>
    public int RoomsCount { get; set; }

    /// <summary>
    /// Идентификатор владельца сервера (может быть null, если владелец не указан).
    /// </summary>
    public Guid? OwnerId { get; init; }

    /// <summary>
    /// Флаг, указывающий, включен ли сервер.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// URL-адрес сервера.
    /// </summary>
    public required Uri Url { get; set; }
}