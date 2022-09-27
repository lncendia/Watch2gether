using Watch2gether.Domain.Comments;
using Watch2gether.Domain.Comments.Ordering;
using Watch2gether.Domain.Comments.Ordering.Visitor;
using Watch2gether.Domain.Ordering.Abstractions;
using Watch2gether.Infrastructure.PersistentStorage.Models;
using Watch2gether.Infrastructure.PersistentStorage.Models.Comments;
using Watch2gether.Infrastructure.PersistentStorage.Visitors.Sorting.Models;

namespace Watch2gether.Infrastructure.PersistentStorage.Visitors.Sorting;

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