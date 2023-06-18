using Overoom.Domain.Films.Entities;
using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.Films.Ordering.Visitor;

public interface IFilmSortingVisitor : ISortingVisitor<IFilmSortingVisitor, Film>
{
    void Visit(OrderByRating order);
    void Visit(OrderByDate order);
}