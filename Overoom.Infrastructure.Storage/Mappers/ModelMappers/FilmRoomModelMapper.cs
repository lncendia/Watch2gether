using Microsoft.EntityFrameworkCore;
using Overoom.Domain.Room.FilmRoom.Entities;
using Overoom.Infrastructure.Storage.Context;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;
using Overoom.Infrastructure.Storage.Models.Room.FilmRoom;

namespace Overoom.Infrastructure.Storage.Mappers.ModelMappers;

internal class FilmRoomModelMapper : IModelMapperUnit<FilmRoomModel, FilmRoom>
{
    private readonly ApplicationDbContext _context;

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

        RoomModelInitializer.InitRoomModel(filmRoom, entity);

        
        filmRoom.Viewers.RemoveAll(x =>
            entity.Viewers.All(m => m.Id != x.EntityId));

        foreach (var viewer in entity.Viewers)
        {
            var viewerModel = filmRoom.Viewers.Cast<FilmViewerModel>().FirstOrDefault(x => x.EntityId == viewer.Id) ??
                              new FilmViewerModel();
            viewerModel.Season = viewer.Season;
            viewerModel.Series = viewer.Series;
            RoomModelInitializer.InitViewerModel(viewerModel, viewer);
            if (viewerModel.Id == default) filmRoom.Viewers.Add(viewerModel);
        }

        return filmRoom;
    }
}