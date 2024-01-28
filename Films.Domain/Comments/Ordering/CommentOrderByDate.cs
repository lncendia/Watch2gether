using Films.Domain.Comments.Ordering.Visitor;
using Films.Domain.Ordering.Abstractions;

namespace Films.Domain.Comments.Ordering;

public class CommentOrderByDate : IOrderBy<Entities.Comment, ICommentSortingVisitor>
{
    public IEnumerable<Entities.Comment> Order(IEnumerable<Entities.Comment> items) => items.OrderBy(x => x.CreatedAt);

    public IList<IEnumerable<Entities.Comment>> Divide(IEnumerable<Entities.Comment> items) =>
        Order(items).GroupBy(x => x.CreatedAt).Select(x => x.AsEnumerable()).ToList();

    public void Accept(ICommentSortingVisitor visitor) => visitor.Visit(this);
}