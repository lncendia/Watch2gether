using Overoom.Infrastructure.PersistentStorage.Models.Playlists;
using Overoom.Infrastructure.PersistentStorage.Visitors.Sorting.Models;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Playlists;
using Overoom.Domain.Playlists.Ordering;
using Overoom.Domain.Playlists.Ordering.Visitor;
using Overoom.Infrastructure.PersistentStorage.Models;

namespace Overoom.Infrastructure.PersistentStorage.Visitors.Sorting;

public class PlaylistSortingVisitor : BaseSortingVisitor<PlaylistModel, IPlaylistSortingVisitor, Playlist>,
    IPlaylistSortingVisitor
{
    public void Visit(OrderByUpdateDate order) => SortItems.Add(new SortData<PlaylistModel>(x => x.Updated, false));

    public void Visit(OrderByCountFilms order) =>
        SortItems.Add(new SortData<PlaylistModel>(x => x.FilmsList.Length, false));

    protected override List<SortData<PlaylistModel>> ConvertOrderToList(
        IOrderBy<Playlist, IPlaylistSortingVisitor> spec)
    {
        var visitor = new PlaylistSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }
}