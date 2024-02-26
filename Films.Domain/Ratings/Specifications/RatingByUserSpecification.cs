using Films.Domain.Ratings.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Ratings.Specifications;

public class RatingByUserSpecification(Guid userId) : ISpecification<Rating, IRatingSpecificationVisitor>
{
    public Guid UserId { get; } = userId;

    public void Accept(IRatingSpecificationVisitor visitor) => visitor.Visit(this);
    public bool IsSatisfiedBy(Rating item) => item.UserId == UserId;
}