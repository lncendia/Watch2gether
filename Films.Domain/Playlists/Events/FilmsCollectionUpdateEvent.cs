using Films.Domain.Abstractions;

namespace Films.Domain.Playlists.Events;

public class FilmsCollectionUpdateEvent : IDomainEvent
{
    public required Guid Id { get; init; }
    public required IReadOnlyCollection<Guid> Films { get; init; }
}