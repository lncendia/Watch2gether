using Films.Domain.Rooms.FilmRooms;
using Microsoft.EntityFrameworkCore;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.FilmRooms;
using Films.Infrastructure.Storage.Models.Rooms;

namespace Films.Infrastructure.Storage.Mappers.ModelMappers;

internal class FilmRoomModelMapper(ApplicationDbContext context) : IModelMapperUnit<FilmRoomModel, FilmRoom>
{
    public async Task<FilmRoomModel> MapAsync(FilmRoom aggregate)
    {
        var model = await context.FilmRooms
            .Include(r => r.Viewers)
            .FirstOrDefaultAsync(x => x.Id == aggregate.Id) ?? new FilmRoomModel { Id = aggregate.Id, };
        
        model.ServerId = aggregate.ServerId;
        model.FilmId = aggregate.FilmId;
        model.Code = aggregate.Code;
        model.CdnName = aggregate.CdnName;
        model.CreationDate = aggregate.CreationDate;

        ProcessViewers(aggregate, model);
        ProcessBanned(aggregate, model);

        return model;
    }

    private static void ProcessViewers(FilmRoom aggregate, FilmRoomModel model)
    {
        model.Viewers.RemoveAll(x => aggregate.Viewers.All(m => m != x.UserId));

        var newViewers = aggregate.Viewers
            .Where(x => model.Viewers.All(m => m.UserId != x))
            .Select(x => new ViewerModel<FilmRoomModel> { UserId = x });

        model.Viewers.AddRange(newViewers);
    }
    
    private static void ProcessBanned(FilmRoom aggregate, FilmRoomModel model)
    {
        model.Banned.RemoveAll(x => aggregate.Viewers.All(m => m != x.UserId));

        var newBanned = aggregate.Banned
            .Where(x => model.Banned.All(m => m.UserId != x))
            .Select(x => new BannedModel<FilmRoomModel> { UserId = x });

        model.Banned.AddRange(newBanned);
    }
}