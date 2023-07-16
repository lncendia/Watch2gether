using Overoom.Domain.Ratings.Ordering.Visitor;
using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.Ratings.Ordering;

public class RatingOrderByDate : IOrderBy<Rating, IRatingSortingVisitor>
{
    public IEnumerable<Rating> Order(IEnumerable<Rating> items) => items.OrderBy(x => x.Date);

    public IList<IEnumerable<Rating>> Divide(IEnumerable<Rating> items) =>
        Order(items).GroupBy(x => x.Date).Select(x => x.AsEnumerable()).ToList();

    public void Accept(IRatingSortingVisitor visitor) => visitor.Visit(this);
}