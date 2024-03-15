using Microsoft.EntityFrameworkCore;
using Room.Domain.Rooms.YoutubeRooms;
using Room.Domain.Rooms.YoutubeRooms.Entities;
using Room.Infrastructure.Storage.Context;
using Room.Infrastructure.Storage.Extensions;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Models.YoutubeRooms;

namespace Room.Infrastructure.Storage.Mappers.ModelMappers;

internal class YoutubeRoomModelMapper(ApplicationDbContext context) : IModelMapperUnit<YoutubeRoomModel, YoutubeRoom>
{
    public async Task<YoutubeRoomModel> MapAsync(YoutubeRoom aggregate)
    {
        var model = await context.YoutubeRooms
            .LoadDependencies()
            .FirstOrDefaultAsync(x => x.Id == aggregate.Id) ?? new YoutubeRoomModel { Id = aggregate.Id };

        model.VideoAccess = aggregate.VideoAccess;

        ProcessViewers(aggregate, model);
        ProcessVideos(aggregate, model);

        return model;
    }

    private static void ProcessViewers(YoutubeRoom aggregate, YoutubeRoomModel model)
    {
        // Удаляем эти записи из коллекции в модели EF
        model.Viewers.RemoveAll(viewerModel => aggregate.Viewers.All(viewer => viewerModel.Id != viewer.Id));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newViewers = model.Viewers
            .Where(x => aggregate.Viewers.All(m => x.Id != m.Id))
            .Select(c => new YoutubeViewerModel { Id = c.Id })
            .ToArray();

        foreach (var viewer in model.Viewers)
        {
            ProcessViewer(viewer, aggregate.Viewers.First(m => m.Id == viewer.Id), aggregate.Owner.Id);
        }

        foreach (var viewer in newViewers)
        {
            ProcessViewer(viewer, aggregate.Viewers.First(m => m.Id == viewer.Id), aggregate.Owner.Id);
        }

        model.Viewers.AddRange(newViewers);
    }

    private static void ProcessViewer(YoutubeViewerModel model, YoutubeViewer viewerEntity, Guid ownerId)
    {
        model.VideoId = viewerEntity.VideoId;
        model.Online = viewerEntity.Online;
        model.Pause = viewerEntity.Pause;
        model.TimeLine = viewerEntity.TimeLine;
        model.FullScreen = viewerEntity.FullScreen;
        model.Username = viewerEntity.Username;
        model.Owner = viewerEntity.Id == ownerId;
        model.Beep = viewerEntity.Allows.Beep;
        model.Scream = viewerEntity.Allows.Scream;
        model.Change = viewerEntity.Allows.Change;
        model.PhotoUrl = viewerEntity.PhotoUrl;
    }
    
    private static void ProcessVideos(YoutubeRoom aggregate, YoutubeRoomModel model)
    {
        // Удаляем эти записи из коллекции в модели EF
        model.Videos.RemoveAll(messageModel =>
            aggregate.Videos.All(valueObject => valueObject.Id != messageModel.VideoId));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newVideos = aggregate.Videos
            .Where(valueObject => model.Videos.All(messageModel => messageModel.VideoId != valueObject.Id))
            .Select(c => new VideoModel
            {
                VideoId = c.Id,
                Added = c.Added
            });

        // Добавляем новые записи
        model.Videos.AddRange(newVideos);
    }
}