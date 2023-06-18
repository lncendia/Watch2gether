using Overoom.Domain.Ratings.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Ratings.Specifications;

public abstract class RatingByUserSpecification : ISpecification<Rating, IRatingSpecificationVisitor>
{
    public RatingByUserSpecification(Guid userId) => UserId = userId;

    public Guid UserId { get; }

    public void Accept(IRatingSpecificationVisitor visitor) => visitor.Visit(this);
    public bool IsSatisfiedBy(Rating item) => item.UserId == UserId;
}