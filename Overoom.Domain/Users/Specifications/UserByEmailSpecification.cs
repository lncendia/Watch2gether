using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.Users.Entities;
using Overoom.Domain.Users.Specifications.Visitor;

namespace Overoom.Domain.Users.Specifications;

public class UserByEmailSpecification : ISpecification<User, IUserSpecificationVisitor>
{
    public string Email { get; }
    public UserByEmailSpecification(string email) => Email = email;

    public bool IsSatisfiedBy(User item) => string.Equals(item.Email, Email, StringComparison.CurrentCultureIgnoreCase);

    public void Accept(IUserSpecificationVisitor visitor) => visitor.Visit(this);
}