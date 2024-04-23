using Films.Domain.Films;
using Films.Domain.Films.Ordering;
using Films.Domain.Films.Ordering.Visitor;
using Films.Domain.Ordering.Abstractions;
using Films.Infrastructure.Storage.Models.Films;
using Films.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Films.Infrastructure.Storage.Visitors.Sorting;

public class FilmSortingVisitor : BaseSortingVisitor<FilmModel, IFilmSortingVisitor, Film>, IFilmSortingVisitor
{
    public void Visit(FilmOrderByUserRating order) => SortItems.Add(new SortData<FilmModel>(x => x.UserRating, false));

    public void Visit(FilmOrderByUserRatingCount order) =>
        SortItems.Add(new SortData<FilmModel>(x => x.UserRatingsCount, false));

    public void Visit(FilmOrderByDate order) => SortItems.Add(new SortData<FilmModel>(x => x.Year, false));

    protected override List<SortData<FilmModel>> ConvertOrderToList(IOrderBy<Film, IFilmSortingVisitor> spec)
    {
        var visitor = new FilmSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }
}