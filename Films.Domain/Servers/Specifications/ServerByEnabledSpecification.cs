using Films.Domain.Servers.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Servers.Specifications;

public class ServerByEnabledSpecification(bool isEnabled) : ISpecification<Server, IServerSpecificationVisitor>
{
    public bool IsEnabled { get; } = isEnabled;

    public void Accept(IServerSpecificationVisitor visitor) => visitor.Visit(this);
    public bool IsSatisfiedBy(Server item) => item.IsEnabled == IsEnabled;
}