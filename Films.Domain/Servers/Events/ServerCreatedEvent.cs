using Films.Domain.Abstractions;

namespace Films.Domain.Servers.Events;

public class ServerCreatedEvent : IDomainEvent
{
    public required Guid Id { get; init; }
}