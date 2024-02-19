namespace Room.Domain.Abstractions;

public abstract class AggregateRoot
{
    public required Guid Id { get; init; }
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}