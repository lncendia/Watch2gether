using Films.Domain.Ordering.Abstractions;
using Films.Domain.Playlists.Ordering.Visitor;

namespace Films.Domain.Playlists.Ordering;

public class PlaylistOrderByCount : IOrderBy<Playlist, IPlaylistSortingVisitor>
{
    public IEnumerable<Playlist> Order(IEnumerable<Playlist> items) => items.OrderBy(x => x.Films.Count);

    public IReadOnlyCollection<IEnumerable<Playlist>> Divide(IEnumerable<Playlist> items) =>
        Order(items).GroupBy(x => x.Films.Count).Select(x => x.AsEnumerable()).ToArray();

    public void Accept(IPlaylistSortingVisitor visitor) => visitor.Visit(this);
}