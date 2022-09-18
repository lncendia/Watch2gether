using Watch2gether.Domain.Ordering.Abstractions;

namespace Watch2gether.Domain.Comments.Ordering.Visitor;

public interface ICommentSortingVisitor : ISortingVisitor<ICommentSortingVisitor, Comment>
{
    void Visit(OrderByDate order);
}