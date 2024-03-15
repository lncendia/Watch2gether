using Microsoft.EntityFrameworkCore;
using Room.Domain.Rooms.FilmRooms;
using Room.Domain.Rooms.FilmRooms.Entities;
using Room.Infrastructure.Storage.Context;
using Room.Infrastructure.Storage.Extensions;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Models.FilmRooms;

namespace Room.Infrastructure.Storage.Mappers.ModelMappers;

internal class FilmRoomModelMapper(ApplicationDbContext context) : IModelMapperUnit<FilmRoomModel, FilmRoom>
{
    public async Task<FilmRoomModel> MapAsync(FilmRoom aggregate)
    {
        var model = await context.FilmRooms
            .LoadDependencies()
            .FirstOrDefaultAsync(x => x.Id == aggregate.Id) ?? new FilmRoomModel { Id = aggregate.Id };

        model.Title = aggregate.Title;
        model.CdnUrl = aggregate.Cdn.Url;
        model.CdnName = aggregate.Cdn.Name;
        model.IsSerial = aggregate.IsSerial;

        ProcessViewers(aggregate, model);

        return model;
    }

    private static void ProcessViewers(FilmRoom aggregate, FilmRoomModel model)
    {
        // Удаляем эти записи из коллекции в модели EF
        model.Viewers.RemoveAll(viewerModel => aggregate.Viewers.All(viewer => viewerModel.Id != viewer.Id));

        // Получаем записи, которые есть в сущности, но еще нет в модели EF
        var newViewers = aggregate.Viewers
            .Where(x => model.Viewers.All(m => x.Id != m.Id))
            .Select(c => new FilmViewerModel { Id = c.Id })
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

    private static void ProcessViewer(FilmViewerModel model, FilmViewer viewerEntity, Guid ownerId)
    {
        model.Season = viewerEntity.Season;
        model.Series = viewerEntity.Series;
        model.Online = viewerEntity.Online;
        model.Pause = viewerEntity.Pause;
        model.TimeLine = viewerEntity.TimeLine;
        model.FullScreen = viewerEntity.FullScreen;
        model.Username = viewerEntity.Username;
        model.Beep = viewerEntity.Allows.Beep;
        model.Scream = viewerEntity.Allows.Scream;
        model.Change = viewerEntity.Allows.Change;
        model.PhotoUrl = viewerEntity.PhotoUrl;
        model.Owner = viewerEntity.Id == ownerId;
    }
}