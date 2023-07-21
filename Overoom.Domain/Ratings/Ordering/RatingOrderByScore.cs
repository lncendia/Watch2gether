using Overoom.Domain.Ratings.Ordering.Visitor;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Ratings.Entities;

namespace Overoom.Domain.Ratings.Ordering;

public class RatingOrderByScore : IOrderBy<Rating, IRatingSortingVisitor>
{
    public IEnumerable<Rating> Order(IEnumerable<Rating> items) => items.OrderBy(x => x.Score);

    public IList<IEnumerable<Rating>> Divide(IEnumerable<Rating> items) =>
        Order(items).GroupBy(x => x.Score).Select(x => x.AsEnumerable()).ToList();

    public void Accept(IRatingSortingVisitor visitor) => visitor.Visit(this);
}