using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Ratings;
using Overoom.Domain.Ratings.Ordering.Visitor;
using Overoom.Infrastructure.Storage.Models.Rating;
using Overoom.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Overoom.Infrastructure.Storage.Visitors.Sorting;

public class RatingSortingVisitor : BaseSortingVisitor<RatingModel, IRatingSortingVisitor, Rating>, IRatingSortingVisitor
{
    protected override List<SortData<RatingModel>> ConvertOrderToList(IOrderBy<Rating, IRatingSortingVisitor> spec)
    {
        var visitor = new RatingSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }
}