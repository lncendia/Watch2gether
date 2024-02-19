using Films.Domain.Ordering.Abstractions;
using Films.Domain.Rooms.FilmRooms;
using Films.Domain.Rooms.FilmRooms.Ordering;
using Films.Domain.Rooms.FilmRooms.Ordering.Visitor;
using Films.Infrastructure.Storage.Models.Rooms.FilmRoom;
using Films.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Films.Infrastructure.Storage.Visitors.Sorting;

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

    public void Visit(FilmRoomOrderByViewersCount order) =>
        SortItems.Add(new SortData<FilmRoomModel>(f => f.Viewers.Count, false));
}