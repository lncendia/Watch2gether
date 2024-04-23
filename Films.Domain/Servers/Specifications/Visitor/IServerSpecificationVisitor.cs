using Films.Domain.Specifications.Abstractions;

namespace Films.Domain.Servers.Specifications.Visitor;

public interface IServerSpecificationVisitor : ISpecificationVisitor<IServerSpecificationVisitor, Server>
{
    void Visit(ServerByEnabledSpecification specification);
    void Visit(ServersByIdsSpecification specification);
}