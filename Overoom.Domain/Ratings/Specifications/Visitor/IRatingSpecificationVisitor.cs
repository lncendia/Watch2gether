using Overoom.Domain.Ratings.Entities;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Ratings.Specifications.Visitor;

public interface IRatingSpecificationVisitor : ISpecificationVisitor<IRatingSpecificationVisitor, Rating>
{
    void Visit(RatingByUserSpecification specification);
    void Visit(RatingByFilmSpecification specification);
}