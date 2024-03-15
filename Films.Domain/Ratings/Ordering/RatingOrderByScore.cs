using Films.Domain.Ordering.Abstractions;
using Films.Domain.Ratings.Ordering.Visitor;

namespace Films.Domain.Ratings.Ordering;

public class RatingOrderByScore : IOrderBy<Rating, IRatingSortingVisitor>
{
    public IEnumerable<Rating> Order(IEnumerable<Rating> items) => items.OrderBy(x => x.Score);

    public IReadOnlyCollection<IEnumerable<Rating>> Divide(IEnumerable<Rating> items) =>
        Order(items).GroupBy(x => x.Score).Select(x => x.AsEnumerable()).ToArray();

    public void Accept(IRatingSortingVisitor visitor) => visitor.Visit(this);
}