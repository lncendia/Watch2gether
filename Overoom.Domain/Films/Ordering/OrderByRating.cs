using Overoom.Domain.Films.Ordering.Visitor;
using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.Films.Ordering;

public class OrderByRating : IOrderBy<Film, IFilmSortingVisitor>
{
    public IEnumerable<Film> Order(IEnumerable<Film> items) => items.OrderBy(x => x.FilmInfo.Rating);
    public IList<IEnumerable<Film>> Divide(IEnumerable<Film> items) =>
        Order(items).GroupBy(x => x.FilmInfo.Rating).Select(x => x.AsEnumerable()).ToList();

    public void Accept(IFilmSortingVisitor visitor) => visitor.Visit(this);
}