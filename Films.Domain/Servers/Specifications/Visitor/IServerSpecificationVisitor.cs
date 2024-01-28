using Films.Domain.Servers.Entities;
using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Servers.Specifications.Visitor;

public interface IServerSpecificationVisitor : ISpecificationVisitor<IServerSpecificationVisitor, Server>
{
    void Visit(ServerByUserSpecification specification);
    void Visit(ServerByEnabledSpecification specification);
}