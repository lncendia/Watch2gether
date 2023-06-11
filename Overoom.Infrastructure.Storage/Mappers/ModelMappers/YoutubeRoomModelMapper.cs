using Microsoft.EntityFrameworkCore;
using Overoom.Domain.Room.YoutubeRoom.Entities;
using Overoom.Infrastructure.Storage.Context;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;
using Overoom.Infrastructure.Storage.Models.Room.YoutubeRoom;

namespace Overoom.Infrastructure.Storage.Mappers.ModelMappers;

internal class YoutubeRoomModelMapper : IModelMapperUnit<YoutubeRoomModel, YoutubeRoom>
{
    private readonly ApplicationDbContext _context;

    public YoutubeRoomModelMapper(ApplicationDbContext context) => _context = context;

    public async Task<YoutubeRoomModel> MapAsync(YoutubeRoom entity)
    {
        var youtubeRoom = await _context.YoutubeRooms.FirstOrDefaultAsync(x => x.Id == entity.Id);
        if (youtubeRoom != null)
        {
            await _context.Entry(youtubeRoom).Collection(x => x.Messages).LoadAsync();
            await _context.Entry(youtubeRoom).Collection(x => x.Viewers).LoadAsync();
        }
        else
        {
            youtubeRoom = new YoutubeRoomModel
            {
                Id = entity.Id,
                OwnerId = entity.Owner.Id
            };
        }

        RoomModelInitializer.InitRoomModel(youtubeRoom, entity);
        youtubeRoom.AddAccess = entity.AddAccess;

        var newIds = entity.VideoIds.Where(x => youtubeRoom.VideoIds.All(m => m.VideoId != x));
        youtubeRoom.VideoIds.AddRange(newIds.Select(x => new VideoIdModel { VideoId = x }));
        youtubeRoom.VideoIds.RemoveAll(x => entity.VideoIds.All(m => m != x.VideoId));


        youtubeRoom.Viewers.RemoveAll(x =>
            entity.Viewers.All(m => m.Id != x.EntityId));

        foreach (var viewer in entity.Viewers)
        {
            var viewerModel =
                youtubeRoom.Viewers.Cast<YoutubeViewerModel>().FirstOrDefault(x => x.EntityId == viewer.Id) ??
                new YoutubeViewerModel();
            viewerModel.CurrentVideoId = viewer.CurrentVideoId;
            RoomModelInitializer.InitViewerModel(viewerModel, viewer);
            if (viewerModel.Id == default) youtubeRoom.Viewers.Add(viewerModel);
        }

        return youtubeRoom;
    }
}