using Films.Domain.Servers.Entities;
using Films.Domain.Servers.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Servers.Specifications;

public class ServerByUserSpecification : ISpecification<Server, IServerSpecificationVisitor>
{
    public required Guid UserId { get; init; }

    public void Accept(IServerSpecificationVisitor visitor) => visitor.Visit(this);
    public bool IsSatisfiedBy(Server item) => item.OwnerId == UserId;
}