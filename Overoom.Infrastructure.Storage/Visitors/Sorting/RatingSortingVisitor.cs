using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Ratings.Entities;
using Overoom.Domain.Ratings.Ordering;
using Overoom.Domain.Ratings.Ordering.Visitor;
using Overoom.Infrastructure.Storage.Models.Rating;
using Overoom.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Overoom.Infrastructure.Storage.Visitors.Sorting;

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