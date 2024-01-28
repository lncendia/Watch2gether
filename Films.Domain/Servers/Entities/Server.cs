using Films.Domain.Abstractions;
using Films.Domain.Servers.Events;

namespace Films.Domain.Servers.Entities;

public class Server : AggregateRoot
{
    public Server()
    {
        AddDomainEvent(new ServerCreatedEvent
        {
            Id = Id
        });
    }

    public required int MaxRoomsCount { get; set; }
    public int RoomsCount { get; set; }
    public Guid? OwnerId { get; init; }
    public bool IsEnabled { get; set; }
    public required Uri Url { get; set; }
}