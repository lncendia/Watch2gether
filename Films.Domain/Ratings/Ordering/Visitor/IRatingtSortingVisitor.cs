using Films.Domain.Ordering.Abstractions;
using Films.Domain.Ratings.Entities;

namespace Films.Domain.Ratings.Ordering.Visitor;

public interface IRatingSortingVisitor : ISortingVisitor<IRatingSortingVisitor, Rating>
{
    public void Visit(RatingOrderByDate order);
    public void Visit(RatingOrderByScore order);
}