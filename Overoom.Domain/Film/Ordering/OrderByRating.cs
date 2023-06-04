using Overoom.Domain.Film.Ordering.Visitor;
using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.Film.Ordering;

public class OrderByRating : IOrderBy<Film.Entities.Film, IFilmSortingVisitor>
{
    public IEnumerable<Film.Entities.Film> Order(IEnumerable<Film.Entities.Film> items) => items.OrderBy(x => x.FilmInfo.RatingKp);
    public IList<IEnumerable<Film.Entities.Film>> Divide(IEnumerable<Film.Entities.Film> items) =>
        Order(items).GroupBy(x => x.FilmInfo.RatingKp).Select(x => x.AsEnumerable()).ToList();

    public void Accept(IFilmSortingVisitor visitor) => visitor.Visit(this);
}