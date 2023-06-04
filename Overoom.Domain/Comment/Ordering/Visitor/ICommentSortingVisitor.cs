using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.Comment.Ordering.Visitor;

public interface ICommentSortingVisitor : ISortingVisitor<ICommentSortingVisitor, Entities.Comment>
{
    void Visit(OrderByDate order);
}