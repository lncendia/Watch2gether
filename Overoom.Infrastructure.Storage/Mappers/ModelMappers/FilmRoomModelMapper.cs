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
        typeof(Room).GetField("_idCounter", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public FilmRoomModelMapper(ApplicationDbContext context) => _context = context;

    public async Task<FilmRoomModel> MapAsync(FilmRoom entity)
    {
        var filmRoom = _context.FilmRooms.Local.FirstOrDefault(x => x.Id == entity.Id);
        if (filmRoom == null)
        {
            filmRoom = await _context.FilmRooms.FirstOrDefaultAsync(x => x.Id == entity.Id);
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
        }

        filmRoom.IsOpen = entity.IsOpen;
        filmRoom.LastActivity = entity.LastActivity;
        filmRoom.IdCounter = (int)IdCounter.GetValue(entity)!;

        var newMessages = entity.Messages.Where(x =>
            filmRoom.Messages.All(m => m.Viewer.EntityId != x.ViewerId || m.CreatedAt != x.CreatedAt));
        filmRoom.Messages.AddRange(newMessages.Select(x => new FilmMessageModel
        {
            Text = x.Text, CreatedAt = x.CreatedAt,
            Viewer = filmRoom.Viewers.First(v => v.EntityId == x.ViewerId)
        }));
        _context.FilmRoomMessages.RemoveRange(filmRoom.Messages.Where(x =>
            entity.Messages.All(m => m.ViewerId != x.Viewer.EntityId && m.CreatedAt != x.CreatedAt)));


        _context.FilmRoomViewers.RemoveRange(filmRoom.Viewers.Where(x => entity.Viewers.All(m => m.Id != x.EntityId)));

        foreach (var viewer in entity.Viewers)
        {
            var viewerModel = filmRoom.Viewers.FirstOrDefault(x => x.EntityId == viewer.Id) ?? new FilmViewerModel
            {
                EntityId = viewer.Id,
                AvatarUri = viewer.AvatarUri
            };
            viewerModel.Name = viewer.Name;
            viewerModel.Season = viewer.Season;
            viewerModel.Series = viewer.Series;
            viewerModel.Online = viewer.Online;
            viewerModel.Pause = viewer.Pause;
            viewerModel.TimeLine = viewer.TimeLine;
            viewerModel.FullScreen = viewer.FullScreen;
            viewerModel.Beep = viewer.Allows.Beep;
            viewerModel.Scream = viewer.Allows.Scream;
            viewerModel.Change = viewer.Allows.Change;
            if (viewerModel.EntityId == default) filmRoom.Viewers.Add(viewerModel); //todo: fix that
        }

        return filmRoom;
    }
}