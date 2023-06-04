using Overoom.Domain.Comment.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Comment.Specifications;

public abstract class UserCommentsSpecification : ISpecification<Entities.Comment, ICommentSpecificationVisitor>
{
    public UserCommentsSpecification(Guid userId) => UserId = userId;

    public Guid UserId { get; }
    public bool IsSatisfiedBy(Entities.Comment item) => item.UserId == UserId;

    public void Accept(ICommentSpecificationVisitor visitor) => visitor.Visit(this);
}