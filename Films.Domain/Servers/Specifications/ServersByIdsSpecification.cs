using Films.Domain.Servers.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Servers.Specifications;

public class ServersByIdsSpecification(IEnumerable<Guid> ids) : ISpecification<Server, IServerSpecificationVisitor>
{
    public IEnumerable<Guid> Ids { get; } = ids;
    public bool IsSatisfiedBy(Server item) => Ids.Any(x => item.Id == x);

    public void Accept(IServerSpecificationVisitor visitor) => visitor.Visit(this);
}