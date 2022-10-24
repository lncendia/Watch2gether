using Overoom.Domain.Films.Ordering.Visitor;
using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.Films.Ordering;

public class OrderByDate : IOrderBy<Film, IFilmSortingVisitor>
{
    public IEnumerable<Film> Order(IEnumerable<Film> items) => items.OrderBy(x => x.Date);

    public IList<IEnumerable<Film>> Divide(IEnumerable<Film> items) =>
        Order(items).GroupBy(x => x.Date).Select(x => x.AsEnumerable()).ToList();

    public void Accept(IFilmSortingVisitor visitor) => visitor.Visit(this);
}