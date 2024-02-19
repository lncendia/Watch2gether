using System.Linq.Expressions;
using Films.Domain.Ratings;
using Films.Domain.Ratings.Specifications;
using Films.Domain.Ratings.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using Films.Infrastructure.Storage.Models.Rating;

namespace Films.Infrastructure.Storage.Visitors.Specifications;

public class RatingVisitor : BaseVisitor<RatingModel, IRatingSpecificationVisitor, Rating>, IRatingSpecificationVisitor
{
    protected override Expression<Func<RatingModel, bool>> ConvertSpecToExpression(
        ISpecification<Rating, IRatingSpecificationVisitor> spec)
    {
        var visitor = new RatingVisitor();
        spec.Accept(visitor);
        return visitor.Expr!;
    }

    public void Visit(RatingByUserSpecification specification) => Expr = x => x.UserId == specification.UserId;
    public void Visit(RatingByFilmSpecification specification) => Expr = x => x.FilmId == specification.FilmId;
}