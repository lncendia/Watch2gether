using Watch2gether.Domain.Ordering.Abstractions;

namespace Watch2gether.Domain.Ordering;

public class DescendingOrder<T, TVisitor> : IOrderBy<T, TVisitor>
    where TVisitor : ISortingVisitor<TVisitor, T>
{
    public IOrderBy<T, TVisitor> OrderData { get; }

    public DescendingOrder(IOrderBy<T, TVisitor> orderData) => OrderData = orderData;

    public IEnumerable<T> Order(IEnumerable<T> items) => OrderData.Order(items).Reverse();
    public IList<IEnumerable<T>> Divide(IEnumerable<T> items)
    {
        var data = OrderData.Divide(items);
        return data.Select(x => x.Reverse()).ToList();
    }

    public void Accept(TVisitor visitor) => visitor.Visit(this);
}