using Overoom.Domain.Rating.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Rating.Specifications;

public abstract class UserRatingsSpecification : ISpecification<Rating, IRatingSpecificationVisitor>
{
    public UserRatingsSpecification(Guid userId) => UserId = userId;

    public Guid UserId { get; }
    public bool IsSatisfiedBy(Comment.Entities.Comment item) => item.UserId == UserId;

    public void Accept(IRatingSpecificationVisitor visitor) => visitor.Visit(this);
    public bool IsSatisfiedBy(Rating item) => item.UserId == UserId;
}