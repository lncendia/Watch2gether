using Films.Domain.Ordering.Abstractions;
using Films.Domain.Servers.Entities;
using Films.Domain.Servers.Ordering;
using Films.Domain.Servers.Ordering.Visitor;
using Films.Infrastructure.Storage.Models.Server;
using Films.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Films.Infrastructure.Storage.Visitors.Sorting;

public class ServerSortingVisitor : BaseSortingVisitor<ServerModel, IServerSortingVisitor, Server>,
    IServerSortingVisitor
{
    protected override List<SortData<ServerModel>> ConvertOrderToList(
        IOrderBy<Server, IServerSortingVisitor> spec)
    {
        var visitor = new ServerSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }

    public void Visit(ServerOrderByCountRooms order)=>
        SortItems.Add(new SortData<ServerModel>(f => f.RoomsCount, false));
}