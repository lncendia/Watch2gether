using Films.Domain.Rooms.YoutubeRooms;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Rooms;
using Films.Infrastructure.Storage.Models.YoutubeRoom;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Storage.Mappers.ModelMappers;

internal class YoutubeRoomModelMapper(ApplicationDbContext context) : IModelMapperUnit<YoutubeRoomModel, YoutubeRoom>
{
    public async Task<YoutubeRoomModel> MapAsync(YoutubeRoom aggregate)
    {
        var model = await context.YoutubeRooms
            .Include(r => r.Viewers)
            .FirstOrDefaultAsync(x => x.Id == aggregate.Id) ?? new YoutubeRoomModel { Id = aggregate.Id };

        model.ServerId = aggregate.ServerId;
        model.Code = aggregate.Code;

        ProcessUsers(aggregate, model);
        ProcessBanned(aggregate, model);

        return model;
    }

    private static void ProcessUsers(YoutubeRoom aggregate, YoutubeRoomModel model)
    {
        model.Viewers.RemoveAll(x => aggregate.Viewers.All(m => m != x.UserId));

        var newViewers = aggregate.Viewers
            .Where(x => model.Viewers.All(m => m.UserId != x))
            .Select(x => new ViewerModel<YoutubeRoomModel> { UserId = x });

        model.Viewers.AddRange(newViewers);
    }
    
    private static void ProcessBanned(YoutubeRoom aggregate, YoutubeRoomModel model)
    {
        model.Banned.RemoveAll(x => aggregate.Viewers.All(m => m != x.UserId));

        var newBanned = aggregate.Banned
            .Where(x => model.Banned.All(m => m.UserId != x))
            .Select(x => new BannedModel<YoutubeRoomModel> { UserId = x });

        model.Banned.AddRange(newBanned);
    }
}