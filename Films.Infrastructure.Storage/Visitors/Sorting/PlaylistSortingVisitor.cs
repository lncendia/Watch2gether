using Films.Domain.Ordering.Abstractions;
using Films.Domain.Playlists;
using Films.Domain.Playlists.Ordering;
using Films.Domain.Playlists.Ordering.Visitor;
using Films.Infrastructure.Storage.Models.Playlist;
using Films.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Films.Infrastructure.Storage.Visitors.Sorting;

public class PlaylistSortingVisitor : BaseSortingVisitor<PlaylistModel, IPlaylistSortingVisitor, Playlist>,
    IPlaylistSortingVisitor
{
    public void Visit(PlaylistOrderByUpdateDate order) =>
        SortItems.Add(new SortData<PlaylistModel>(x => x.Updated, false));

    public void Visit(PlaylistOrderByCount order) =>
        SortItems.Add(new SortData<PlaylistModel>(x => x.Films.Count, false));

    protected override List<SortData<PlaylistModel>> ConvertOrderToList(
        IOrderBy<Playlist, IPlaylistSortingVisitor> spec)
    {
        var visitor = new PlaylistSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }
}