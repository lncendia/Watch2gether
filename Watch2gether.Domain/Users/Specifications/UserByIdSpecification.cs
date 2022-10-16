using Watch2gether.Domain.Specifications.Abstractions;
using Watch2gether.Domain.Users.Specifications.Visitor;

namespace Watch2gether.Domain.Users.Specifications;

public class UserByIdSpecification : ISpecification<User, IUserSpecificationVisitor>
{
    public Guid Id { get; }
    public UserByIdSpecification(Guid id) => Id = id;

    public bool IsSatisfiedBy(User item) => item.Id == Id;

    public void Accept(IUserSpecificationVisitor visitor) => visitor.Visit(this);
}