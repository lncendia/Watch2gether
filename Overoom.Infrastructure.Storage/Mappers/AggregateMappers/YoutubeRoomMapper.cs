using System.Reflection;
using Overoom.Domain.Rooms.YoutubeRoom.Entities;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;
using Overoom.Infrastructure.Storage.Models.Room.YoutubeRoom;

namespace Overoom.Infrastructure.Storage.Mappers.AggregateMappers;

internal class YoutubeRoomMapper : IAggregateMapperUnit<YoutubeRoom, YoutubeRoomModel>
{
    private static readonly Type YoutubeRoomType = typeof(YoutubeRoom);
    private static readonly Type YoutubeViewerType = typeof(YoutubeViewer);

    private static readonly FieldInfo Ids =
        YoutubeRoomType.GetField("_ids", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public YoutubeRoom Map(YoutubeRoomModel model)
    {
        var room = new YoutubeRoom("mockUrl", "mockName", "mockUri", model.AddAccess);
        Ids.SetValue(room, model.VideoIds.Select(x => x.VideoId).ToList());
        var viewers = model.Viewers.Cast<YoutubeViewerModel>().Select(CreateViewer);
        RoomInitializer.InitRoom(room, model, viewers, model.OwnerId);
        return room;
    }

    private static YoutubeViewer CreateViewer(YoutubeViewerModel model)
    {
        object?[] args = { model.EntityId, model.Name, model.AvatarUri, model.CurrentVideoId };
        var element = (YoutubeViewer)YoutubeViewerType.Assembly.CreateInstance(
            YoutubeViewerType.FullName!, false, BindingFlags.Instance | BindingFlags.NonPublic, null, args!,
            null, null)!;
        RoomInitializer.InitViewer(element, model);
        return element;
    }
}