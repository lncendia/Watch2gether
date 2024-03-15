using Room.Domain.Ordering.Abstractions;

namespace Room.Domain.Ordering;

public class DescendingOrder<T, TVisitor>(IOrderBy<T, TVisitor> orderData) : IOrderBy<T, TVisitor>
    where TVisitor : ISortingVisitor<TVisitor, T>
{
    public IOrderBy<T, TVisitor> OrderData { get; } = orderData;

    public IEnumerable<T> Order(IEnumerable<T> items) => OrderData.Order(items).Reverse();
    public IReadOnlyCollection<IEnumerable<T>> Divide(IEnumerable<T> items)
    {
        var data = OrderData.Divide(items);
        return data.Select(x => x.Reverse()).ToArray();
    }

    public void Accept(TVisitor visitor) => visitor.Visit(this);
}