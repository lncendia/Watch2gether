using System.Collections.Generic;
using System.Linq;
using Films.Domain.Ordering.Abstractions;
using Films.Domain.Rooms.YoutubeRooms.Ordering.Visitor;

namespace Films.Domain.Rooms.YoutubeRooms.Ordering;

public class YoutubeRoomOrderByViewersCount : IOrderBy<YoutubeRoom, IYoutubeRoomSortingVisitor>
{
    public IEnumerable<YoutubeRoom> Order(IEnumerable<YoutubeRoom> items) => items.OrderBy(x => x.Viewers.Count);

    public IList<IEnumerable<YoutubeRoom>> Divide(IEnumerable<YoutubeRoom> items) =>
        Order(items).GroupBy(x => x.Viewers.Count).Select(x => x.AsEnumerable()).ToList();

    public void Accept(IYoutubeRoomSortingVisitor visitor) => visitor.Visit(this);
}