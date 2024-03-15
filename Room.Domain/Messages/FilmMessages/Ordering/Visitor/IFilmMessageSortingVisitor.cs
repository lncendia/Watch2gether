using Room.Domain.Ordering.Abstractions;

namespace Room.Domain.Messages.FilmMessages.Ordering.Visitor;

public interface IFilmMessageSortingVisitor : ISortingVisitor<IFilmMessageSortingVisitor, FilmMessage>
{
    void Visit(MessagesOrderByDate order);
}