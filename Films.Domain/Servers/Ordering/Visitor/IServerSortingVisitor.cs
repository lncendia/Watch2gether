using Films.Domain.Ordering.Abstractions;
using Films.Domain.Servers.Entities;

namespace Films.Domain.Servers.Ordering.Visitor;

public interface IServerSortingVisitor : ISortingVisitor<IServerSortingVisitor, Server>
{
    void Visit(ServerOrderByCountRooms order);
}