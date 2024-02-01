using Films.Domain.Abstractions;
using Films.Domain.Playlists.Entities;

namespace Films.Domain.Playlists.Events;

public class FilmsCollectionUpdateEvent(Playlist playlist) : IDomainEvent
{
    public Playlist Playlist { get; } = playlist;
}