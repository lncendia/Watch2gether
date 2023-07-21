using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.Users.Entities;
using Overoom.Domain.Users.Specifications.Visitor;

namespace Overoom.Domain.Users.Specifications;

public class UserByIdsSpecification : ISpecification<User, IUserSpecificationVisitor>
{
    public IEnumerable<Guid> Ids { get; }
    public UserByIdsSpecification(IEnumerable<Guid> ids) => Ids = ids;

    public bool IsSatisfiedBy(User item) => Ids.Any(x => x == item.Id);

    public void Accept(IUserSpecificationVisitor visitor) => visitor.Visit(this);
}