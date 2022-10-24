using Overoom.Infrastructure.PersistentStorage.Models.Films;
using Overoom.Infrastructure.PersistentStorage.Visitors.Sorting.Models;
using Overoom.Domain.Films;
using Overoom.Domain.Films.Ordering;
using Overoom.Domain.Films.Ordering.Visitor;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Infrastructure.PersistentStorage.Models;

namespace Overoom.Infrastructure.PersistentStorage.Visitors.Sorting;

public class FilmSortingVisitor : BaseSortingVisitor<FilmModel, IFilmSortingVisitor, Film>, IFilmSortingVisitor
{
    public void Visit(OrderByRating order) => SortItems.Add(new SortData<FilmModel>(x => x.Rating, false));

    public void Visit(OrderByDate order) => SortItems.Add(new SortData<FilmModel>(x => x.Date, false));

    protected override List<SortData<FilmModel>> ConvertOrderToList(IOrderBy<Film, IFilmSortingVisitor> spec)
    {
        var visitor = new FilmSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }
}