using Overoom.Domain.Films.Entities;
using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.Films.Ordering.Visitor;

public interface IFilmSortingVisitor : ISortingVisitor<IFilmSortingVisitor, Film>
{
    void Visit(FilmOrderByRating order);
    void Visit(FilmOrderByUserRating order);
    void Visit(FilmOrderByUserRatingCount order);
    void Visit(FilmOrderByDate order);
}