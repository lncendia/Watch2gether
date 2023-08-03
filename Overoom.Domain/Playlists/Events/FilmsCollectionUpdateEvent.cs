using Overoom.Domain.Abstractions;

namespace Overoom.Domain.Playlists.Events;

public class FilmsCollectionUpdateEvent : IDomainEvent
{
    public FilmsCollectionUpdateEvent(IReadOnlyCollection<Guid> films) => Films = films;

    public IReadOnlyCollection<Guid> Films { get; }
}