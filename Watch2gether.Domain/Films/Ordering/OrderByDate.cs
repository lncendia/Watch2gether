using Watch2gether.Domain.Films.Ordering.Visitor;
using Watch2gether.Domain.Ordering.Abstractions;

namespace Watch2gether.Domain.Films.Ordering;

public class OrderByDate : IOrderBy<Film, IFilmSortingVisitor>
{
    public IEnumerable<Film> Order(IEnumerable<Film> items) => items.OrderBy(x => x.FilmData.Year);

    public IList<IEnumerable<Film>> Divide(IEnumerable<Film> items) =>
        Order(items).GroupBy(x => x.FilmData.Year).Select(x => x.AsEnumerable()).ToList();

    public void Accept(IFilmSortingVisitor visitor) => visitor.Visit(this);
}