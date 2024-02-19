using System.Collections.Generic;
using System.Linq;
using Films.Domain.Ordering.Abstractions;
using Films.Domain.Playlists.Ordering.Visitor;

namespace Films.Domain.Playlists.Ordering;

public class PlaylistOrderByUpdateDate : IOrderBy<Playlist, IPlaylistSortingVisitor>
{
    public IEnumerable<Playlist> Order(IEnumerable<Playlist> items) => items.OrderBy(x => x.Updated);

    public IList<IEnumerable<Playlist>> Divide(IEnumerable<Playlist> items) =>
        Order(items).GroupBy(x => x.Updated).Select(x => x.AsEnumerable()).ToList();

    public void Accept(IPlaylistSortingVisitor visitor) => visitor.Visit(this);
}