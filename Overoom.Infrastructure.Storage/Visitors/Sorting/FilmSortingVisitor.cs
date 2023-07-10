using Overoom.Domain.Films.Entities;
using Overoom.Domain.Films.Ordering;
using Overoom.Domain.Films.Ordering.Visitor;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Infrastructure.Storage.Models.Film;
using Overoom.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Overoom.Infrastructure.Storage.Visitors.Sorting;

public class FilmSortingVisitor : BaseSortingVisitor<FilmModel, IFilmSortingVisitor, Film>, IFilmSortingVisitor
{
    public void Visit(FilmOrderByRating order) => SortItems.Add(new SortData<FilmModel>(x => x.Rating, false));

    public void Visit(FilmOrderByDate order) => SortItems.Add(new SortData<FilmModel>(x => x.Year, false));

    protected override List<SortData<FilmModel>> ConvertOrderToList(IOrderBy<Film, IFilmSortingVisitor> spec)
    {
        var visitor = new FilmSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }
}