using Films.Domain.Ordering.Abstractions;

namespace Films.Domain.Comments.Ordering.Visitor;

public interface ICommentSortingVisitor : ISortingVisitor<ICommentSortingVisitor, Entities.Comment>
{
    void Visit(CommentOrderByDate order);
}