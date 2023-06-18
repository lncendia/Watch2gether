using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.Users.Entities;
using Overoom.Domain.Users.Specifications.Visitor;

namespace Overoom.Domain.Users.Specifications;

public class UserByIdSpecification : ISpecification<User, IUserSpecificationVisitor>
{
    public Guid Id { get; }
    public UserByIdSpecification(Guid id) => Id = id;

    public bool IsSatisfiedBy(User item) => item.Id == Id;

    public void Accept(IUserSpecificationVisitor visitor) => visitor.Visit(this);
}