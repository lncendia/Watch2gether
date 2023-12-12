using System.Reflection;
using Overoom.Domain.Rooms.BaseRoom.DTOs;
using Overoom.Domain.Rooms.YoutubeRoom.Entities;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;
using Overoom.Infrastructure.Storage.Models.YoutubeRoom;

namespace Overoom.Infrastructure.Storage.Mappers.AggregateMappers;

internal class YoutubeRoomMapper : IAggregateMapperUnit<YoutubeRoom, YoutubeRoomModel>
{
    private static readonly Type YoutubeRoomType = typeof(YoutubeRoom);
    private static readonly Type YoutubeViewerType = typeof(YoutubeViewer);
    private static readonly ViewerDto MockViewer = new("mock", new Uri("mock", UriKind.Relative));
    private static readonly Uri MockUri = new Uri("https://www.youtube.com/watch?v=75QStdC3WgA");

    private static readonly FieldInfo Ids =
        YoutubeRoomType.GetField("_ids", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public YoutubeRoom Map(YoutubeRoomModel model)
    {
        var room = new YoutubeRoom(MockUri, model.Access, model.IsOpen, MockViewer);
        Ids.SetValue(room, model.VideoIds.Select(x => x.VideoId).ToList());
        var viewers = model.Viewers.Select(CreateViewer);
        var messages = model.Messages.Select(x =>
            RoomInitializer.CreateMessage(x.Viewer.EntityId, x.Text, x.CreatedAt));
        RoomInitializer.InitRoom(room, model.Id, model.LastActivity, model.IdCounter, viewers, model.OwnerId, messages);
        return room;
    }

    private static YoutubeViewer CreateViewer(YoutubeViewerModel model)
    {
        var dto = RoomInitializer.CreateViewerDto(model.Name, model.AvatarUri, model.Beep, model.Scream, model.Change);
        object?[] args = { dto, model.EntityId, model.CurrentVideoId };
        var element = (YoutubeViewer)YoutubeViewerType.Assembly.CreateInstance(
            YoutubeViewerType.FullName!, false, BindingFlags.Instance | BindingFlags.NonPublic, null, args!,
            null, null)!;
        RoomInitializer.InitViewer(element, model.Online, model.Pause, model.FullScreen, model.TimeLine);
        return element;
    }
}