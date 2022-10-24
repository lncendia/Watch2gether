using Overoom.Domain.Comments.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Domain.Comments.Specifications;

public abstract class UserCommentsSpecification : ISpecification<Comment, ICommentSpecificationVisitor>
{
    public UserCommentsSpecification(Guid userId) => UserId = userId;

    public Guid UserId { get; }
    public bool IsSatisfiedBy(Comment item) => item.UserId == UserId;

    public void Accept(ICommentSpecificationVisitor visitor) => visitor.Visit(this);
}