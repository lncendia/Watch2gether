using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Overoom.Domain.Rooms.BaseRoom.Entities;
using Overoom.Domain.Rooms.YoutubeRoom.Entities;
using Overoom.Infrastructure.Storage.Context;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Models.Room.YoutubeRoom;
using Overoom.Infrastructure.Storage.Models.YoutubeRoom;

namespace Overoom.Infrastructure.Storage.Mappers.ModelMappers;

internal class YoutubeRoomModelMapper : IModelMapperUnit<YoutubeRoomModel, YoutubeRoom>
{
    private readonly ApplicationDbContext _context;

    private static readonly FieldInfo IdCounter =
        typeof(Room).GetField("IdCounter", BindingFlags.Instance | BindingFlags.NonPublic)!;

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

        youtubeRoom.IsOpen = entity.IsOpen;
        youtubeRoom.LastActivity = entity.LastActivity;
        youtubeRoom.IdCounter = (int)IdCounter.GetValue(entity)!;
        youtubeRoom.AddAccess = entity.AddAccess;

        var newMessages = entity.Messages.Where(x =>
            youtubeRoom.Messages.All(m => m.ViewerEntityId != x.ViewerId && m.CreatedAt != x.CreatedAt));
        youtubeRoom.Messages.AddRange(newMessages.Select(x => new YoutubeMessageModel
            { ViewerEntityId = x.ViewerId, Text = x.Text, CreatedAt = x.CreatedAt }));
        _context.YoutubeMessages.RemoveRange(youtubeRoom.Messages.Where(x =>
            entity.Messages.All(m => m.ViewerId != x.ViewerEntityId && m.CreatedAt != x.CreatedAt)));

        var newIds = entity.VideoIds.Where(x => youtubeRoom.VideoIds.All(m => m.VideoId != x));
        youtubeRoom.VideoIds.AddRange(newIds.Select(x => new VideoIdModel { VideoId = x }));
        _context.VideoIds.RemoveRange(youtubeRoom.VideoIds.Where(x => entity.VideoIds.All(m => m != x.VideoId)));


        _context.YoutubeViewers.RemoveRange(youtubeRoom.Viewers.Where(x =>
            entity.Viewers.All(m => m.Id != x.EntityId)));

        foreach (var viewer in entity.Viewers)
        {
            var viewerModel =
                youtubeRoom.Viewers.FirstOrDefault(x => x.EntityId == viewer.Id) ??
                new YoutubeViewerModel
                {
                    EntityId = viewer.Id,
                    Name = viewer.Name,
                    NameNormalized = viewer.Name.ToUpper(),
                    AvatarUri = viewer.AvatarUri
                };
            viewerModel.CurrentVideoId = viewer.CurrentVideoId;
            viewerModel.Online = viewer.Online;
            viewerModel.OnPause = viewer.OnPause;
            viewerModel.TimeLine = viewer.TimeLine;
            if (viewerModel.Id == default) youtubeRoom.Viewers.Add(viewerModel);
        }

        return youtubeRoom;
    }
}