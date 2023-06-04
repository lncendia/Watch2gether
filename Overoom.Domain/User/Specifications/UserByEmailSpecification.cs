using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.User.Specifications.Visitor;

namespace Overoom.Domain.User.Specifications;

public class UserByEmailSpecification : ISpecification<User.Entities.User, IUserSpecificationVisitor>
{
    public string Email { get; }
    public UserByEmailSpecification(string email) => Email = email;

    public bool IsSatisfiedBy(User.Entities.User item) => item.Email == Email;

    public void Accept(IUserSpecificationVisitor visitor) => visitor.Visit(this);
}