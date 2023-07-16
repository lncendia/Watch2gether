using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.Ratings.Ordering.Visitor;

public interface IRatingSortingVisitor : ISortingVisitor<IRatingSortingVisitor, Rating>
{
    public void Visit(RatingOrderByDate order);
}