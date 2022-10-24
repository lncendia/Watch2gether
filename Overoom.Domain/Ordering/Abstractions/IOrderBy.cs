namespace Overoom.Domain.Ordering.Abstractions;

public interface IOrderBy<T, in TVisitor> where TVisitor : ISortingVisitor<TVisitor, T>
{
    IEnumerable<T> Order(IEnumerable<T> items);
    IList<IEnumerable<T>> Divide(IEnumerable<T> items);
    void Accept(TVisitor visitor);
}