using System.Reflection;
using Films.Domain.Servers;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Mappers.StaticMethods;
using Films.Infrastructure.Storage.Models.Servers;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Storage.Mappers.AggregateMappers;

internal class ServerMapper(ApplicationDbContext context) : IAggregateMapperUnit<Server, ServerModel>
{
    private static readonly FieldInfo RoomsCount =
        typeof(Server).GetField("<RoomsCount>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public async Task<Server> MapAsync(ServerModel model)
    {
        var room = new Server
        {
            MaxRoomsCount = model.MaxRoomsCount,
            Url = model.Url,
            IsEnabled = model.IsEnabled
        };

        var filmRoomsCount = await context.FilmRooms.CountAsync(x => x.ServerId == model.Id);
        var youtubeRoomsCount = await context.YoutubeRooms.CountAsync(x => x.ServerId == model.Id);
        RoomsCount.SetValue(room, filmRoomsCount + youtubeRoomsCount);
        IdFields.AggregateId.SetValue(room, model.Id);
        return room;
    }
}