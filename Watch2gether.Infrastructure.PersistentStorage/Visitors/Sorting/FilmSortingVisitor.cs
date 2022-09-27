using Watch2gether.Domain.Films;
using Watch2gether.Domain.Films.Ordering;
using Watch2gether.Domain.Films.Ordering.Visitor;
using Watch2gether.Domain.Ordering.Abstractions;
using Watch2gether.Infrastructure.PersistentStorage.Models;
using Watch2gether.Infrastructure.PersistentStorage.Models.Films;
using Watch2gether.Infrastructure.PersistentStorage.Visitors.Sorting.Models;

namespace Watch2gether.Infrastructure.PersistentStorage.Visitors.Sorting;

public class FilmSortingVisitor : BaseSortingVisitor<FilmModel, IFilmSortingVisitor, Film>, IFilmSortingVisitor
{
    public void Visit(OrderByRating order) => SortItems.Add(new SortData<FilmModel>(x => x.Rating, false));

    public void Visit(OrderByDate order) => SortItems.Add(new SortData<FilmModel>(x => x.Year, false));

    protected override List<SortData<FilmModel>> ConvertOrderToList(IOrderBy<Film, IFilmSortingVisitor> spec)
    {
        var visitor = new FilmSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }
}