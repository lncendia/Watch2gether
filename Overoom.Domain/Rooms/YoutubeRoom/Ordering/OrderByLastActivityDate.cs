using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Rooms.YoutubeRoom.Ordering.Visitor;

namespace Overoom.Domain.Rooms.YoutubeRoom.Ordering;

public class OrderByLastActivityDate : IOrderBy<YoutubeRoom, IYoutubeRoomSortingVisitor>
{
    public IEnumerable<YoutubeRoom> Order(IEnumerable<YoutubeRoom> items) => items.OrderBy(x => x.LastActivity);

    public IList<IEnumerable<YoutubeRoom>> Divide(IEnumerable<YoutubeRoom> items) =>
        Order(items).GroupBy(x => x.LastActivity).Select(x => x.AsEnumerable()).ToList();

    public void Accept(IYoutubeRoomSortingVisitor visitor) => visitor.Visit(this);
}