using Films.Domain.Ordering.Abstractions;

namespace Films.Domain.Ratings.Ordering.Visitor;

public interface IRatingSortingVisitor : ISortingVisitor<IRatingSortingVisitor, Rating>
{
    public void Visit(RatingOrderByDate order);
    public void Visit(RatingOrderByScore order);
}