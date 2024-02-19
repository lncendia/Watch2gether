using Microsoft.EntityFrameworkCore;
using Room.Domain.BaseRoom.ValueObjects;
using Room.Domain.YoutubeRooms;
using Room.Domain.YoutubeRooms.Entities;
using Room.Infrastructure.Storage.Context;
using Room.Infrastructure.Storage.Extensions;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Models.BaseRoom;
using Room.Infrastructure.Storage.Models.YoutubeRoom;

namespace Room.Infrastructure.Storage.Mappers.ModelMappers;

internal class YoutubeRoomModelMapper(ApplicationDbContext context) : IModelMapperUnit<YoutubeRoomModel, YoutubeRoom>
{
    public async Task<YoutubeRoomModel> MapAsync(YoutubeRoom aggregate)
    {
        var model = await context.YoutubeRooms
            .LoadDependencies()
            .FirstOrDefaultAsync(x => x.Id == aggregate.Id) ?? new YoutubeRoomModel { Id = aggregate.Id };

        model.VideoAccess = aggregate.VideoAccess;
        model.LastActivity = aggregate.LastActivity;

        ProcessViewers(aggregate, model);
        ProcessMessages(aggregate, model);
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
        model.Nickname = viewerEntity.Nickname;
        model.Owner = viewerEntity.Id == ownerId;
        model.Beep = viewerEntity.Allows.Beep;
        model.Scream = viewerEntity.Allows.Scream;
        model.Change = viewerEntity.Allows.Change;
    }

    private static void ProcessMessages(YoutubeRoom aggregate, YoutubeRoomModel model)
    {
        // Удаляем эти записи из коллекции в модели EF
        model.Messages.RemoveAll(messageModel => aggregate.Messages.All(valueObject => Compare(valueObject, messageModel)));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newMessages = aggregate.Messages
            .Where(valueObject => model.Messages.All(messageModel => Compare(valueObject, messageModel)))
            .Select(c => new MessageModel<YoutubeRoomModel>
            {
                CreatedAt = c.CreatedAt,
                Text = c.Text,
                ViewerId = c.ViewerId
            })
            .ToArray();

        model.Messages.AddRange(newMessages);
        return;

        bool Compare(Message valueObject, MessageModel<YoutubeRoomModel> messageModel)
        {
            return messageModel.ViewerId != valueObject.ViewerId && messageModel.CreatedAt != valueObject.CreatedAt;
        }
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