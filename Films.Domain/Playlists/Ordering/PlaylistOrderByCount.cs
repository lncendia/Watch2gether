using Films.Domain.Ordering.Abstractions;
using Films.Domain.Playlists.Entities;
using Films.Domain.Playlists.Ordering.Visitor;

namespace Films.Domain.Playlists.Ordering;

public class PlaylistOrderByCount : IOrderBy<Entities.Playlist, IPlaylistSortingVisitor>
{
    public IEnumerable<Playlist> Order(IEnumerable<Entities.Playlist> items) => items.OrderBy(x => x.Films.Count);

    public IList<IEnumerable<Entities.Playlist>> Divide(IEnumerable<Entities.Playlist> items) =>
        Order(items).GroupBy(x => x.Films.Count).Select(x => x.AsEnumerable()).ToList();

    public void Accept(IPlaylistSortingVisitor visitor) => visitor.Visit(this);
}