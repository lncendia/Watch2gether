using Overoom.Domain.Abstractions;
using Overoom.Domain.Playlists.Entities;

namespace Overoom.Domain.Playlists.Events;

public class FilmsCollectionUpdateEvent : IDomainEvent
{
    public FilmsCollectionUpdateEvent(Playlist playlist) => Playlist = playlist;

    public Playlist Playlist { get; }
}