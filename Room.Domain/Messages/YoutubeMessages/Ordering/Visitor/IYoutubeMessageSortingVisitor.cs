using Room.Domain.Ordering.Abstractions;

namespace Room.Domain.Messages.YoutubeMessages.Ordering.Visitor;

public interface IYoutubeMessageSortingVisitor : ISortingVisitor<IYoutubeMessageSortingVisitor, YoutubeMessage>
{
    void Visit(MessagesOrderByDate order);
}