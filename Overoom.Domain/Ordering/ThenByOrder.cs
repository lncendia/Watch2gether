using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.Ordering;

public class ThenByOrder<T, TVisitor> : IOrderBy<T, TVisitor>
    where TVisitor : ISortingVisitor<TVisitor, T>
{
    public IOrderBy<T, TVisitor> Left { get; }
    public IOrderBy<T, TVisitor> Right { get; }

    public ThenByOrder(IOrderBy<T, TVisitor> left, IOrderBy<T, TVisitor> right)
    {
        Left = left;
        Right = right;
    }

    public IEnumerable<T> Order(IEnumerable<T> items)
    {
        var list = Left.Divide(items);
        return list.SelectMany(Right.Order);
    }

    public IList<IEnumerable<T>> Divide(IEnumerable<T> items)
    {
        var list = Left.Divide(items);
        return list.Select(Right.Order).ToList();
    }

    public void Accept(TVisitor visitor) => visitor.Visit(this);
}