using Room.Domain.Ordering.Abstractions;
using Room.Domain.Rooms.FilmRooms;
using Room.Domain.Rooms.FilmRooms.Ordering.Visitor;
using Room.Infrastructure.Storage.Models.FilmRooms;
using Room.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Room.Infrastructure.Storage.Visitors.Sorting;

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
}