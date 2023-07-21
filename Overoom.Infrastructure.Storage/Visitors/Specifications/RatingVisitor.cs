using System.Linq.Expressions;
using Overoom.Domain.Ratings;
using Overoom.Domain.Ratings.Entities;
using Overoom.Domain.Ratings.Specifications;
using Overoom.Domain.Ratings.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Infrastructure.Storage.Models.Rating;

namespace Overoom.Infrastructure.Storage.Visitors.Specifications;

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