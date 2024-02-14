using Microsoft.EntityFrameworkCore;
using Room.Domain.Rooms.BaseRoom.ValueObjects;
using Room.Domain.Rooms.YoutubeRoom.Entities;
using Room.Infrastructure.Storage.Context;
using Room.Infrastructure.Storage.Extensions;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Models.Room.Base;
using Room.Infrastructure.Storage.Models.Room.YoutubeRoom;

namespace Room.Infrastructure.Storage.Mappers.ModelMappers;

internal class YoutubeRoomModelMapper(ApplicationDbContext context) : IModelMapperUnit<YoutubeRoomModel, YoutubeRoom>
{
    public async Task<YoutubeRoomModel> MapAsync(YoutubeRoom entity)
    {
        var youtubeRoom = await context.YoutubeRooms
            .LoadDependencies()
            .FirstOrDefaultAsync(x => x.Id == entity.Id) ?? new YoutubeRoomModel { Id = entity.Id };

        youtubeRoom.Code = entity.Code;
        youtubeRoom.LastActivity = entity.LastActivity;

        ProcessViewers(entity, youtubeRoom);
        ProcessMessages(entity, youtubeRoom);
        ProcessVideos(entity, youtubeRoom);

        return youtubeRoom;
    }

    private static void ProcessViewers(YoutubeRoom room, YoutubeRoomModel model)
    {
        // Удаляем эти записи из коллекции в модели EF
        model.Viewers.RemoveAll(viewerModel => room.Viewers.All(viewer => viewerModel.UserId != viewer.UserId));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newViewers = model.Viewers
            .Where(x => room.Viewers.All(m => x.UserId != m.UserId))
            .Select(c => new YoutubeViewerModel { UserId = c.UserId })
            .ToArray();

        foreach (var viewer in model.Viewers)
        {
            ProcessViewer(viewer, room.Viewers.First(m => m.UserId == viewer.UserId), room.Owner.UserId);
        }

        foreach (var viewer in newViewers)
        {
            ProcessViewer(viewer, room.Viewers.First(m => m.UserId == viewer.UserId), room.Owner.UserId);
        }

        model.Viewers.AddRange(newViewers);
    }

    private static void ProcessViewer(YoutubeViewerModel model, YoutubeViewer viewer, Guid ownerId)
    {
        model.VideoId = viewer.VideoId;
        model.Online = viewer.Online;
        model.Pause = viewer.Pause;
        model.TimeLine = viewer.TimeLine;
        model.FullScreen = viewer.FullScreen;
        model.CustomName = viewer.CustomName;
        model.Owner = viewer.UserId == ownerId;
    }

    private static void ProcessMessages(YoutubeRoom room, YoutubeRoomModel model)
    {
        // Удаляем эти записи из коллекции в модели EF
        model.Messages.RemoveAll(messageModel => room.Messages.All(valueObject => Compare(valueObject, messageModel)));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newMessages = room.Messages
            .Where(valueObject => model.Messages.All(messageModel => Compare(valueObject, messageModel)))
            .Select(c => new MessageModel<YoutubeRoomModel>
            {
                CreatedAt = c.CreatedAt,
                Text = c.Text,
                UserId = c.UserId
            })
            .ToArray();

        model.Messages.AddRange(newMessages);
        return;

        bool Compare(Message valueObject, MessageModel<YoutubeRoomModel> messageModel)
        {
            return messageModel.UserId != valueObject.UserId && messageModel.CreatedAt != valueObject.CreatedAt;
        }
    }

    private static void ProcessVideos(YoutubeRoom room, YoutubeRoomModel model)
    {
        // Удаляем эти записи из коллекции в модели EF
        model.Videos.RemoveAll(messageModel =>
            room.Videos.All(valueObject => valueObject.Id != messageModel.VideoId));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newVideos = room.Videos
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