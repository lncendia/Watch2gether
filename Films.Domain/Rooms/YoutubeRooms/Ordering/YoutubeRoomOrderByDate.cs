using Films.Domain.Ordering.Abstractions;
using Films.Domain.Rooms.YoutubeRooms.Ordering.Visitor;

namespace Films.Domain.Rooms.YoutubeRooms.Ordering;

public class YoutubeRoomOrderByDate : IOrderBy<YoutubeRoom, IYoutubeRoomSortingVisitor>
{
    public IEnumerable<YoutubeRoom> Order(IEnumerable<YoutubeRoom> items) => items.OrderBy(x => x.CreationDate);

    public IReadOnlyCollection<IEnumerable<YoutubeRoom>> Divide(IEnumerable<YoutubeRoom> items) =>
        Order(items).GroupBy(x => x.CreationDate).Select(x => x.AsEnumerable()).ToArray();

    public void Accept(IYoutubeRoomSortingVisitor visitor) => visitor.Visit(this);
}