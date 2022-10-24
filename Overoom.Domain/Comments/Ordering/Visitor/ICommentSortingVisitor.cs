using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.Comments.Ordering.Visitor;

public interface ICommentSortingVisitor : ISortingVisitor<ICommentSortingVisitor, Comment>
{
    void Visit(OrderByDate order);
}