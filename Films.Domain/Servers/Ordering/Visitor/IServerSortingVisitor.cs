using Films.Domain.Ordering.Abstractions;

namespace Films.Domain.Servers.Ordering.Visitor;

public interface IServerSortingVisitor : ISortingVisitor<IServerSortingVisitor, Server>
{
    void Visit(ServerOrderByCountRooms order);
}