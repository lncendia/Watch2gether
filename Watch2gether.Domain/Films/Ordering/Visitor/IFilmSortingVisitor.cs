using Watch2gether.Domain.Ordering.Abstractions;

namespace Watch2gether.Domain.Films.Ordering.Visitor;

public interface IFilmSortingVisitor : ISortingVisitor<IFilmSortingVisitor, Film>
{
    void Visit(OrderByRating order);
    void Visit(OrderByDate order);
}