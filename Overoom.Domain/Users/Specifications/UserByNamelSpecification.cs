using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.Users.Entities;
using Overoom.Domain.Users.Specifications.Visitor;

namespace Overoom.Domain.Users.Specifications;

public class UserByNameSpecification : ISpecification<User, IUserSpecificationVisitor>
{
    public string Name { get; }
    public UserByNameSpecification(string name) => Name = name;

    public bool IsSatisfiedBy(User item) => item.Name.Contains(Name);

    public void Accept(IUserSpecificationVisitor visitor) => visitor.Visit(this);
}