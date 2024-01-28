using Films.Domain.Ordering.Abstractions;
using Films.Domain.Ratings.Entities;
using Films.Domain.Ratings.Ordering.Visitor;

namespace Films.Domain.Ratings.Ordering;

public class RatingOrderByDate : IOrderBy<Rating, IRatingSortingVisitor>
{
    public IEnumerable<Rating> Order(IEnumerable<Rating> items) => items.OrderBy(x => x.Date);

    public IList<IEnumerable<Rating>> Divide(IEnumerable<Rating> items) =>
        Order(items).GroupBy(x => x.Date).Select(x => x.AsEnumerable()).ToList();

    public void Accept(IRatingSortingVisitor visitor) => visitor.Visit(this);
}