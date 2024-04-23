using Films.Domain.Servers;
using Microsoft.EntityFrameworkCore;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Servers;

namespace Films.Infrastructure.Storage.Mappers.ModelMappers;

internal class ServerModelMapper(ApplicationDbContext context) : IModelMapperUnit<ServerModel, Server>
{
    public async Task<ServerModel> MapAsync(Server aggregate)
    {
        var model = await context.Servers.FirstOrDefaultAsync(x => x.Id == aggregate.Id) ?? new ServerModel
        {
            Id = aggregate.Id,
        };
        model.RoomsCount = aggregate.RoomsCount;
        model.IsEnabled = aggregate.IsEnabled;
        model.Url = aggregate.Url;
        model.MaxRoomsCount = aggregate.MaxRoomsCount;
        return model;
    }
}