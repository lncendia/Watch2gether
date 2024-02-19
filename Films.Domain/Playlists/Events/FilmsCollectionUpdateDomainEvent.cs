using Films.Domain.Abstractions;

namespace Films.Domain.Playlists.Events;

public class FilmsCollectionUpdateDomainEvent(Playlist playlist) : IDomainEvent
{
    public Playlist Playlist { get; } = playlist;
}