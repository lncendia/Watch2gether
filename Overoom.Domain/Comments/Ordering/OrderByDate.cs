using Overoom.Domain.Comments.Ordering.Visitor;
using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.Comments.Ordering;

public class OrderByDate : IOrderBy<Entities.Comment, ICommentSortingVisitor>
{
    public IEnumerable<Entities.Comment> Order(IEnumerable<Entities.Comment> items) => items.OrderBy(x => x.CreatedAt);

    public IList<IEnumerable<Entities.Comment>> Divide(IEnumerable<Entities.Comment> items) =>
        Order(items).GroupBy(x => x.CreatedAt).Select(x => x.AsEnumerable()).ToList();

    public void Accept(ICommentSortingVisitor visitor) => visitor.Visit(this);
}