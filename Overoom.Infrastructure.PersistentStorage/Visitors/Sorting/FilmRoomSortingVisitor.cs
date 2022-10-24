using Overoom.Infrastructure.PersistentStorage.Models.Rooms;
using Overoom.Infrastructure.PersistentStorage.Visitors.Sorting.Models;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Rooms.FilmRoom;
using Overoom.Domain.Rooms.FilmRoom.Ordering;
using Overoom.Domain.Rooms.FilmRoom.Ordering.Visitor;

namespace Overoom.Infrastructure.PersistentStorage.Visitors.Sorting;

public class FilmRoomSortingVisitor : BaseSortingVisitor<FilmRoomModel, IFilmRoomSortingVisitor, FilmRoom>,
    IFilmRoomSortingVisitor
{
    protected override List<SortData<FilmRoomModel>> ConvertOrderToList(
        IOrderBy<FilmRoom, IFilmRoomSortingVisitor> spec)
    {
        var visitor = new FilmRoomSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }

    public void Visit(OrderByLastActivityDate sorting) =>
        SortItems.Add(new SortData<FilmRoomModel>(f => f.LastActivity, false));
}