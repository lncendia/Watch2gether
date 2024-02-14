using Microsoft.EntityFrameworkCore;
using Room.Domain.Rooms.BaseRoom.ValueObjects;
using Room.Domain.Rooms.FilmRoom.Entities;
using Room.Infrastructure.Storage.Context;
using Room.Infrastructure.Storage.Extensions;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Models.Room.Base;
using Room.Infrastructure.Storage.Models.Room.FilmRoom;

namespace Room.Infrastructure.Storage.Mappers.ModelMappers;

internal class FilmRoomModelMapper(ApplicationDbContext context) : IModelMapperUnit<FilmRoomModel, FilmRoom>
{
    public async Task<FilmRoomModel> MapAsync(FilmRoom entity)
    {
        var filmRoom = await context.FilmRooms
            .LoadDependencies()
            .FirstOrDefaultAsync(x => x.Id == entity.Id) ?? new FilmRoomModel { Id = entity.Id };

        filmRoom.FilmId = entity.FilmId;
        filmRoom.CdnName = entity.CdnName;
        filmRoom.Code = entity.Code;
        filmRoom.LastActivity = entity.LastActivity;

        ProcessViewers(entity, filmRoom);
        ProcessMessages(entity, filmRoom);

        return filmRoom;
    }

    private static void ProcessViewers(FilmRoom room, FilmRoomModel model)
    {
        // Удаляем эти записи из коллекции в модели EF
        model.Viewers.RemoveAll(viewerModel => room.Viewers.All(viewer => viewerModel.UserId != viewer.UserId));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newViewers = room.Viewers
            .Where(x => model.Viewers.All(m => x.UserId != m.UserId))
            .Select(c => new FilmViewerModel { UserId = c.UserId })
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

    private static void ProcessViewer(FilmViewerModel model, FilmViewer viewer, Guid ownerId)
    {
        model.Season = viewer.Season;
        model.Series = viewer.Series;
        model.Online = viewer.Online;
        model.Pause = viewer.Pause;
        model.TimeLine = viewer.TimeLine;
        model.FullScreen = viewer.FullScreen;
        model.CustomName = viewer.CustomName;
        model.Owner = viewer.UserId == ownerId;
    }

    private static void ProcessMessages(FilmRoom room, FilmRoomModel model)
    {
        // Удаляем эти записи из коллекции в модели EF
        model.Messages.RemoveAll(messageModel => room.Messages.All(valueObject => Compare(valueObject, messageModel)));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newMessages = room.Messages
            .Where(valueObject => model.Messages.All(messageModel => Compare(valueObject, messageModel)))
            .Select(c => new MessageModel<FilmRoomModel>
            {
                CreatedAt = c.CreatedAt,
                Text = c.Text,
                UserId = c.UserId
            })
            .ToArray();

        model.Messages.AddRange(newMessages);
        return;

        bool Compare(Message valueObject, MessageModel<FilmRoomModel> messageModel)
        {
            return messageModel.UserId != valueObject.UserId && messageModel.CreatedAt != valueObject.CreatedAt;
        }
    }
}