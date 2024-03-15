namespace Room.Domain.Ordering.Abstractions;

public interface IOrderBy<T, in TVisitor> where TVisitor : ISortingVisitor<TVisitor, T>
{
    IEnumerable<T> Order(IEnumerable<T> items);
    IReadOnlyCollection<IEnumerable<T>> Divide(IEnumerable<T> items);
    void Accept(TVisitor visitor);
}