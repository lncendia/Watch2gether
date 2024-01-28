using Films.Domain.Comments.Entities;
using Films.Domain.Comments.Ordering;
using Films.Domain.Comments.Ordering.Visitor;
using Films.Domain.Ordering.Abstractions;
using Films.Infrastructure.Storage.Models.Comment;
using Films.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Films.Infrastructure.Storage.Visitors.Sorting;

public class CommentSortingVisitor : BaseSortingVisitor<CommentModel, ICommentSortingVisitor, Comment>,
    ICommentSortingVisitor
{
    protected override List<SortData<CommentModel>> ConvertOrderToList(IOrderBy<Comment, ICommentSortingVisitor> spec)
    {
        var visitor = new CommentSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }

    public void Visit(CommentOrderByDate order)
    {
        SortItems.Add(new SortData<CommentModel>(x => x.CreatedAt, false));
    }
}