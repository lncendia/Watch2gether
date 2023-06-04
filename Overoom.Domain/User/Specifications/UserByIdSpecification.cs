using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.User.Specifications.Visitor;

namespace Overoom.Domain.User.Specifications;

public class UserByIdSpecification : ISpecification<User.Entities.User, IUserSpecificationVisitor>
{
    public Guid Id { get; }
    public UserByIdSpecification(Guid id) => Id = id;

    public bool IsSatisfiedBy(User.Entities.User item) => item.Id == Id;

    public void Accept(IUserSpecificationVisitor visitor) => visitor.Visit(this);
}