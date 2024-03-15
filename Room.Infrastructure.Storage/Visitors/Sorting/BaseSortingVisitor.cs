using Room.Domain.Ordering;
using Room.Domain.Ordering.Abstractions;
using Room.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Room.Infrastructure.Storage.Visitors.Sorting;

public abstract class BaseSortingVisitor<TEntity, TVisitor, TItem> where TVisitor : ISortingVisitor<TVisitor, TItem>
{
    public List<SortData<TEntity>> SortItems { get; } = [];
    protected abstract List<SortData<TEntity>> ConvertOrderToList(IOrderBy<TItem, TVisitor> spec);

    public void Visit(DescendingOrder<TItem, TVisitor> order)
    {
        var x = ConvertOrderToList(order.OrderData);
        SortItems.AddRange(x.Take(x.Count - 1));
        var last = x.Last();
        SortItems.Add(new SortData<TEntity>(last.Expr, true));
    }

    public void Visit(ThenByOrder<TItem, TVisitor> order)
    {
        var left = ConvertOrderToList(order.Left);
        var right = ConvertOrderToList(order.Right);
        SortItems.AddRange(left);
        SortItems.AddRange(right);
    }

    public void Visit(RandomOrder<TItem, TVisitor> order) =>
        SortItems.Add(new SortData<TEntity>(x => Guid.NewGuid(), false));
}