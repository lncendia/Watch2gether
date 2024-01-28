using Films.Domain.Specifications.Abstractions;
using Films.Domain.Users.Entities;
using Films.Domain.Users.Specifications.Visitor;

namespace Films.Domain.Users.Specifications;

public class UsersByIdsSpecification(IEnumerable<Guid> ids) : ISpecification<User, IUserSpecificationVisitor>
{
    public IEnumerable<Guid> Ids { get; } = ids;

    public bool IsSatisfiedBy(User item) => Ids.Any(x => x == item.Id);

    public void Accept(IUserSpecificationVisitor visitor) => visitor.Visit(this);
}