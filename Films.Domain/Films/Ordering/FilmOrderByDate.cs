using Films.Domain.Films.Ordering.Visitor;
using Films.Domain.Ordering.Abstractions;

namespace Films.Domain.Films.Ordering;

public class FilmOrderByDate : IOrderBy<Film, IFilmSortingVisitor>
{
    public IEnumerable<Film> Order(IEnumerable<Film> items) => items.OrderBy(x => x.Year);

    public IReadOnlyCollection<IEnumerable<Film>> Divide(IEnumerable<Film> items) =>
        Order(items).GroupBy(x => x.Year).Select(x => x.AsEnumerable()).ToArray();

    public void Accept(IFilmSortingVisitor visitor) => visitor.Visit(this);
}