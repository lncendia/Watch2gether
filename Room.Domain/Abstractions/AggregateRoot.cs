namespace Room.Domain.Abstractions;

public abstract class AggregateRoot(Guid id)
{
    public Guid Id { get; } = id;
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}