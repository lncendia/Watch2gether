using Overoom.Domain.Comments.Entities;
using Overoom.Domain.Comments.Ordering;
using Overoom.Domain.Comments.Ordering.Visitor;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Infrastructure.Storage.Models.Comment;
using Overoom.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Overoom.Infrastructure.Storage.Visitors.Sorting;

public class CommentSortingVisitor : BaseSortingVisitor<CommentModel, ICommentSortingVisitor, Comment>,
    ICommentSortingVisitor
{
    protected override List<SortData<CommentModel>> ConvertOrderToList(IOrderBy<Comment, ICommentSortingVisitor> spec)
    {
        var visitor = new CommentSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }

    public void Visit(OrderByDate order)
    {
        SortItems.Add(new SortData<CommentModel>(x => x.CreatedAt, false));
    }
}