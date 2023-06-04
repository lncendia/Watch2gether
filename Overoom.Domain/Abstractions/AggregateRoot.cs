namespace Overoom.Domain.Abstractions;

public abstract class AggregateRoot
{
    public Guid Id { get; }
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected AggregateRoot() => Id = Guid.NewGuid();

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}