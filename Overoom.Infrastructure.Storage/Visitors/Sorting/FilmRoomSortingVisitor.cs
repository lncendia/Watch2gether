using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Rooms.FilmRoom.Entities;
using Overoom.Domain.Rooms.FilmRoom.Ordering;
using Overoom.Domain.Rooms.FilmRoom.Ordering.Visitor;
using Overoom.Infrastructure.Storage.Models.FilmRoom;
using Overoom.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Overoom.Infrastructure.Storage.Visitors.Sorting;

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

    public void Visit(FilmRoomOrderByLastActivityDate sorting) =>
        SortItems.Add(new SortData<FilmRoomModel>(f => f.LastActivity, false));
}