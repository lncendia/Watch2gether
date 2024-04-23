using Room.Domain.Messages.FilmMessages;
using Room.Domain.Messages.FilmMessages.Ordering;
using Room.Domain.Messages.FilmMessages.Ordering.Visitor;
using Room.Domain.Ordering.Abstractions;
using Room.Infrastructure.Storage.Models.FilmRooms;
using Room.Infrastructure.Storage.Models.Messages;
using Room.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Room.Infrastructure.Storage.Visitors.Sorting;

public class FilmMessageSortingVisitor :
    BaseSortingVisitor<MessageModel<FilmRoomModel>, IFilmMessageSortingVisitor, FilmMessage>,
    IFilmMessageSortingVisitor
{
    protected override List<SortData<MessageModel<FilmRoomModel>>> ConvertOrderToList(
        IOrderBy<FilmMessage, IFilmMessageSortingVisitor> spec)
    {
        var visitor = new FilmMessageSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }

    public void Visit(MessagesOrderByDate order)
    {
        SortItems.Add(new SortData<MessageModel<FilmRoomModel>>(m => m.CreatedAt, false));
    }
}