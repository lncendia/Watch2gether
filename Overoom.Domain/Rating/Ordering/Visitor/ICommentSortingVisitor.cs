using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.Rating.Ordering.Visitor;

public interface IRatingSortingVisitor : ISortingVisitor<IRatingSortingVisitor, Rating>
{
}