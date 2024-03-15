using Films.Domain.Ordering.Abstractions;
using Films.Domain.Ratings;
using Films.Domain.Ratings.Ordering;
using Films.Domain.Ratings.Ordering.Visitor;
using Films.Infrastructure.Storage.Models.Ratings;
using Films.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Films.Infrastructure.Storage.Visitors.Sorting;

public class RatingSortingVisitor : BaseSortingVisitor<RatingModel, IRatingSortingVisitor, Rating>,
    IRatingSortingVisitor
{
    protected override List<SortData<RatingModel>> ConvertOrderToList(IOrderBy<Rating, IRatingSortingVisitor> spec)
    {
        var visitor = new RatingSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }

    public void Visit(RatingOrderByDate order) => SortItems.Add(new SortData<RatingModel>(x => x.Date, false));
    public void Visit(RatingOrderByScore order) => SortItems.Add(new SortData<RatingModel>(x => x.Score, false));
}