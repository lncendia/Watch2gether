using Room.Domain.Messages.FilmMessages.Ordering.Visitor;
using Room.Domain.Ordering.Abstractions;

namespace Room.Domain.Messages.FilmMessages.Ordering;

public class MessagesOrderByDate : IOrderBy<FilmMessage, IFilmMessageSortingVisitor>
{
    public IEnumerable<FilmMessage> Order(IEnumerable<FilmMessage> items) => items.OrderBy(x => x.CreatedAt);

    public IReadOnlyCollection<IEnumerable<FilmMessage>> Divide(IEnumerable<FilmMessage> items) =>
        Order(items).GroupBy(x => x.CreatedAt).Select(x => x.AsEnumerable()).ToArray();

    public void Accept(IFilmMessageSortingVisitor visitor) => visitor.Visit(this);
}