using Watch2gether.Domain.Specifications.Abstractions;
using Watch2gether.Domain.Users.Specifications.Visitor;

namespace Watch2gether.Domain.Users.Specifications;

public class UserFromEmailSpecification : ISpecification<User, IUserSpecificationVisitor>
{
    public string Email { get; }
    public UserFromEmailSpecification(string email) => Email = email;

    public bool IsSatisfiedBy(User item) => item.Email == Email;

    public void Accept(IUserSpecificationVisitor visitor) => visitor.Visit(this);
}