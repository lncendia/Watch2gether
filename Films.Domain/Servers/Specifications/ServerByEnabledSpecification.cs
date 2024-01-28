using Films.Domain.Servers.Entities;
using Films.Domain.Servers.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Servers.Specifications;

public class ServerByEnabledSpecification : ISpecification<Server, IServerSpecificationVisitor>
{
    public required bool IsEnabled { get; init; }

    public void Accept(IServerSpecificationVisitor visitor) => visitor.Visit(this);
    public bool IsSatisfiedBy(Server item) => item.IsEnabled == IsEnabled;
}