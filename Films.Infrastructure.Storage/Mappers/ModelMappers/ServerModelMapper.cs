using Microsoft.EntityFrameworkCore;
using Films.Domain.Servers.Entities;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Server;

namespace Films.Infrastructure.Storage.Mappers.ModelMappers;

internal class ServerModelMapper(ApplicationDbContext context) : IModelMapperUnit<ServerModel, Server>
{
    public async Task<ServerModel> MapAsync(Server entity)
    {
        var server = await context.Servers.FirstOrDefaultAsync(x => x.Id == entity.Id) ?? new ServerModel
        {
            Id = entity.Id,
            OwnerId = entity.OwnerId
        };
        server.RoomsCount = entity.RoomsCount;
        server.IsEnabled = entity.IsEnabled;
        server.Url = entity.Url;
        server.MaxRoomsCount = entity.MaxRoomsCount;
        return server;
    }
}