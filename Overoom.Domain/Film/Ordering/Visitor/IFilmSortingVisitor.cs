using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.Film.Ordering.Visitor;

public interface IFilmSortingVisitor : ISortingVisitor<IFilmSortingVisitor, Film.Entities.Film>
{
    void Visit(OrderByRating order);
    void Visit(OrderByDate order);
}