using Films.Domain.Servers.Entities;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Mappers.StaticMethods;
using Films.Infrastructure.Storage.Models.Server;

namespace Films.Infrastructure.Storage.Mappers.AggregateMappers;

internal class ServerMapper : IAggregateMapperUnit<Server, ServerModel>
{
    public Server Map(ServerModel model)
    {
        var room = new Server
        {
            OwnerId = model.OwnerId,
            MaxRoomsCount = model.MaxRoomsCount,
            Url = model.Url,
            IsEnabled = model.IsEnabled
        };

        IdFields.AggregateId.SetValue(room, model.Id);
        return room;
    }
}