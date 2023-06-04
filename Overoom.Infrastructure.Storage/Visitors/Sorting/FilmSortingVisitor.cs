using Overoom.Domain.Film.Entities;
using Overoom.Domain.Film.Ordering;
using Overoom.Domain.Film.Ordering.Visitor;
using Overoom.Domain.Films;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Infrastructure.Storage.Models;
using Overoom.Infrastructure.Storage.Models.Films;
using Overoom.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Overoom.Infrastructure.Storage.Visitors.Sorting;

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