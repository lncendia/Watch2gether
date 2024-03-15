using Room.Domain.Messages.YoutubeMessages;
using Room.Domain.Messages.YoutubeMessages.Ordering;
using Room.Domain.Messages.YoutubeMessages.Ordering.Visitor;
using Room.Domain.Ordering.Abstractions;
using Room.Infrastructure.Storage.Models.YoutubeRooms;
using Room.Infrastructure.Storage.Models.Messages;
using Room.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Room.Infrastructure.Storage.Visitors.Sorting;

public class YoutubeMessageSortingVisitor :
    BaseSortingVisitor<MessageModel<YoutubeRoomModel>, IYoutubeMessageSortingVisitor, YoutubeMessage>,
    IYoutubeMessageSortingVisitor
{
    protected override List<SortData<MessageModel<YoutubeRoomModel>>> ConvertOrderToList(
        IOrderBy<YoutubeMessage, IYoutubeMessageSortingVisitor> spec)
    {
        var visitor = new YoutubeMessageSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }

    public void Visit(MessagesOrderByDate order)
    {
        SortItems.Add(new SortData<MessageModel<YoutubeRoomModel>>(m => m.CreatedAt, false));
    }
}