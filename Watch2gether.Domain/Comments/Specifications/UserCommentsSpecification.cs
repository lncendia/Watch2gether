using Watch2gether.Domain.Comments.Specifications.Visitor;
using Watch2gether.Domain.Specifications.Abstractions;

namespace Watch2gether.Domain.Comments.Specifications;

public class UserCommentsSpecification : ISpecification<Comment, ICommentSpecificationVisitor>
{
    public UserCommentsSpecification(Guid userId) => UserId = userId;

    public Guid UserId { get; }
    public bool IsSatisfiedBy(Comment item) => item.UserId == UserId;

    public void Accept(ICommentSpecificationVisitor visitor) => visitor.Visit(this);
}