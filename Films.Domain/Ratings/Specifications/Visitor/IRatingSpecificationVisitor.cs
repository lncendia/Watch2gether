using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Ratings.Specifications.Visitor;

public interface IRatingSpecificationVisitor : ISpecificationVisitor<IRatingSpecificationVisitor, Rating>
{
    void Visit(RatingByUserSpecification specification);
    void Visit(RatingByFilmSpecification specification);
}