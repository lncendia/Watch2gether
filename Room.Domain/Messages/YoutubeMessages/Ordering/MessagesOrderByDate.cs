using Room.Domain.Messages.YoutubeMessages.Ordering.Visitor;
using Room.Domain.Ordering.Abstractions;

namespace Room.Domain.Messages.YoutubeMessages.Ordering;

public class MessagesOrderByDate : IOrderBy<YoutubeMessage, IYoutubeMessageSortingVisitor>
{
    public IEnumerable<YoutubeMessage> Order(IEnumerable<YoutubeMessage> items) => items.OrderBy(x => x.CreatedAt);

    public IReadOnlyCollection<IEnumerable<YoutubeMessage>> Divide(IEnumerable<YoutubeMessage> items) =>
        Order(items).GroupBy(x => x.CreatedAt).Select(x => x.AsEnumerable()).ToArray();

    public void Accept(IYoutubeMessageSortingVisitor visitor) => visitor.Visit(this);
}