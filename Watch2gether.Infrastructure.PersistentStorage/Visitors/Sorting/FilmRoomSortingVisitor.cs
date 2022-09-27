using Watch2gether.Domain.Ordering.Abstractions;
using Watch2gether.Domain.Rooms.FilmRoom;
using Watch2gether.Domain.Rooms.FilmRoom.Ordering.Visitor;
using Watch2gether.Infrastructure.PersistentStorage.Models.Rooms;
using Watch2gether.Infrastructure.PersistentStorage.Visitors.Sorting.Models;

namespace Watch2gether.Infrastructure.PersistentStorage.Visitors.Sorting;

public class FilmRoomSortingVisitor : BaseSortingVisitor<FilmRoomModel, IFilmRoomSortingVisitor, FilmRoom>, IFilmRoomSortingVisitor
{
    protected override List<SortData<FilmRoomModel>> ConvertOrderToList(IOrderBy<FilmRoom, IFilmRoomSortingVisitor> spec)
    {
        var visitor = new FilmRoomSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }
}