using Overoom.Infrastructure.PersistentStorage.Models.Comments;
using Overoom.Infrastructure.PersistentStorage.Visitors.Sorting.Models;
using Overoom.Domain.Comments;
using Overoom.Domain.Comments.Ordering;
using Overoom.Domain.Comments.Ordering.Visitor;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Infrastructure.PersistentStorage.Models;

namespace Overoom.Infrastructure.PersistentStorage.Visitors.Sorting;

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