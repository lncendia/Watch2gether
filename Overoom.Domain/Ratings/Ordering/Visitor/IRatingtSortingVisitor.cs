using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Ratings.Entities;

namespace Overoom.Domain.Ratings.Ordering.Visitor;

public interface IRatingSortingVisitor : ISortingVisitor<IRatingSortingVisitor, Rating>
{
    public void Visit(RatingOrderByDate order);
    public void Visit(RatingOrderByScore order);
}