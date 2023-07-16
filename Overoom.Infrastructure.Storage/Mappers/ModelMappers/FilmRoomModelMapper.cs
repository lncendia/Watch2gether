using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Overoom.Domain.Rooms.BaseRoom.Entities;
using Overoom.Domain.Rooms.FilmRoom.Entities;
using Overoom.Infrastructure.Storage.Context;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Models.FilmRoom;

namespace Overoom.Infrastructure.Storage.Mappers.ModelMappers;

internal class FilmRoomModelMapper : IModelMapperUnit<FilmRoomModel, FilmRoom>
{
    private readonly ApplicationDbContext _context;

    private static readonly FieldInfo IdCounter =
        typeof(Room).GetField("IdCounter", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public FilmRoomModelMapper(ApplicationDbContext context) => _context = context;

    public async Task<FilmRoomModel> MapAsync(FilmRoom entity)
    {
        var filmRoom = await _context.FilmRooms.FirstOrDefaultAsync(x => x.Id == entity.Id);
        if (filmRoom != null)
        {
            await _context.Entry(filmRoom).Collection(x => x.Messages).LoadAsync();
            await _context.Entry(filmRoom).Collection(x => x.Viewers).LoadAsync();
        }
        else
        {
            filmRoom = new FilmRoomModel
            {
                Id = entity.Id,
                FilmId = entity.FilmId,
                CdnType = entity.CdnType,
                OwnerId = entity.Owner.Id
            };
        }

        filmRoom.IsOpen = entity.IsOpen;
        filmRoom.LastActivity = entity.LastActivity;
        filmRoom.IdCounter = (int)IdCounter.GetValue(entity)!;

        var newMessages = entity.Messages.Where(x =>
            filmRoom.Messages.All(m => m.ViewerEntityId != x.ViewerId && m.CreatedAt != x.CreatedAt));
        filmRoom.Messages.AddRange(newMessages.Select(x => new FilmMessageModel
            { ViewerEntityId = x.ViewerId, Text = x.Text, CreatedAt = x.CreatedAt }));
        _context.FilmMessages.RemoveRange(filmRoom.Messages.Where(x =>
            entity.Messages.All(m => m.ViewerId != x.ViewerEntityId && m.CreatedAt != x.CreatedAt)));


        _context.FilmViewers.RemoveRange(filmRoom.Viewers.Where(x => entity.Viewers.All(m => m.Id != x.EntityId)));

        foreach (var viewer in entity.Viewers)
        {
            var viewerModel = filmRoom.Viewers.FirstOrDefault(x => x.EntityId == viewer.Id) ?? new FilmViewerModel
            {
                EntityId = viewer.Id,
                Name = viewer.Name,
                NameNormalized = viewer.Name.ToUpper(),
                AvatarUri = viewer.AvatarUri
            };
            viewerModel.Season = viewer.Season;
            viewerModel.Series = viewer.Series;
            viewerModel.Online = viewer.Online;
            viewerModel.OnPause = viewer.OnPause;
            viewerModel.TimeLine = viewer.TimeLine;
            if (viewerModel.Id == default) filmRoom.Viewers.Add(viewerModel);
        }

        return filmRoom;
    }
}