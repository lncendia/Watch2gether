using System.Linq.Expressions;
using Films.Domain.Servers;
using Films.Domain.Servers.Specifications;
using Films.Domain.Servers.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using Films.Infrastructure.Storage.Models.Servers;

namespace Films.Infrastructure.Storage.Visitors.Specifications;

public class ServerVisitor : BaseVisitor<ServerModel, IServerSpecificationVisitor, Server>,
    IServerSpecificationVisitor
{
    protected override Expression<Func<ServerModel, bool>> ConvertSpecToExpression(
        ISpecification<Server, IServerSpecificationVisitor> spec)
    {
        var visitor = new ServerVisitor();
        spec.Accept(visitor);
        return visitor.Expr!;
    }

    public void Visit(ServerByEnabledSpecification specification) => Expr = x => x.IsEnabled;
    public void Visit(ServersByIdsSpecification specification) => Expr = x => specification.Ids.Contains(x.Id);
}