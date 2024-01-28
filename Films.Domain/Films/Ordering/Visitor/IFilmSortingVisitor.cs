using Films.Domain.Films.Entities;
using Films.Domain.Ordering.Abstractions;

namespace Films.Domain.Films.Ordering.Visitor;

public interface IFilmSortingVisitor : ISortingVisitor<IFilmSortingVisitor, Film>
{
    void Visit(FilmOrderByUserRating order);
    void Visit(FilmOrderByUserRatingCount order);
    void Visit(FilmOrderByDate order);
}