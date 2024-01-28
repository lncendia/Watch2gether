using Films.Domain.Comments.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Comments.Specifications;

public abstract class UserCommentsSpecification(Guid userId)
    : ISpecification<Entities.Comment, ICommentSpecificationVisitor>
{
    public Guid UserId { get; } = userId;
    public bool IsSatisfiedBy(Entities.Comment item) => item.UserId == UserId;

    public void Accept(ICommentSpecificationVisitor visitor) => visitor.Visit(this);
}