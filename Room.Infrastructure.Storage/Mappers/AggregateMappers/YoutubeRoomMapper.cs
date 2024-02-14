using System.Reflection;
using System.Runtime.CompilerServices;
using Room.Domain.Abstractions;
using Room.Domain.Rooms.BaseRoom.Entities;
using Room.Domain.Rooms.BaseRoom.ValueObjects;
using Room.Domain.Rooms.YoutubeRoom.Entities;
using Room.Domain.Rooms.YoutubeRoom.ValueObjects;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Mappers.StaticMethods;
using Room.Infrastructure.Storage.Models.Room.Base;
using Room.Infrastructure.Storage.Models.Room.YoutubeRoom;

namespace Room.Infrastructure.Storage.Mappers.AggregateMappers;

internal class YoutubeRoomMapper : IAggregateMapperUnit<YoutubeRoom, YoutubeRoomModel>
{
    private static readonly Type YoutubeViewerType = typeof(YoutubeViewer);
    private static readonly Type ViewerType = typeof(Viewer);

    private static readonly Type YoutubeRoomType = typeof(YoutubeRoom);
    private static readonly Type RoomType = typeof(Room<YoutubeViewer>);

    private static readonly Type VideoType = typeof(Video);


    private static readonly FieldInfo VideoAccess =
        YoutubeRoomType.GetField("<VideoAccess>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo Videos =
        YoutubeRoomType.GetField("_videos", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo LastActivity =
        RoomType.GetField("<LastActivity>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo Code =
        RoomType.GetField("<Code>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo Owner =
        RoomType.GetField("<Owner>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo ViewersList =
        RoomType.GetField("_viewersList", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo MessagesList =
        RoomType.GetField("_messagesList", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo BannedList =
        RoomType.GetField("_bannedList", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo ViewerUserId =
        ViewerType.GetField("<UserId>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo ViewerOnline =
        ViewerType.GetField("<Online>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo ViewerFullScreen =
        ViewerType.GetField("<FullScreen>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo ViewerPause =
        ViewerType.GetField("<Pause>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo ViewerTimeLine =
        ViewerType.GetField("<TimeLine>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo ViewerName =
        ViewerType.GetField("_name", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo ViewerVideoId =
        YoutubeViewerType.GetField("<VideoId>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo MessageCreatedAt =
        typeof(Message).GetField("<CreatedAt>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo VideoId =
        VideoType.GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo VideoAdded =
        VideoType.GetField("<Added>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public YoutubeRoom Map(YoutubeRoomModel model)
    {
        var room = (YoutubeRoom)RuntimeHelpers.GetUninitializedObject(YoutubeRoomType);
        var viewers = model.Viewers.Select(CreateViewer).ToList();
        var messages = model.Messages.Select(CreateMessage).ToList();
        var bannedList = model.BannedUsers.Select(b => b.UserId).ToList();
        var videos = model.Videos.Select(CreateVideo).ToList();

        var owner = viewers.First(v => v.UserId == model.Viewers.First(m => m.Owner).UserId);

        VideoAccess.SetValue(room, model.Access);
        Videos.SetValue(room, videos);
        LastActivity.SetValue(room, model.LastActivity);
        Code.SetValue(room, model.Code);
        Owner.SetValue(room, owner);
        ViewersList.SetValue(room, viewers);
        MessagesList.SetValue(room, messages);
        BannedList.SetValue(room, bannedList);
        IdFields.AggregateId.SetValue(room, model.Id);
        IdFields.DomainEvents.SetValue(room, new List<IDomainEvent>());
        return room;
    }

    private static YoutubeViewer CreateViewer(YoutubeViewerModel model)
    {
        var viewer = (YoutubeViewer)RuntimeHelpers.GetUninitializedObject(YoutubeViewerType);
        ViewerUserId.SetValue(viewer, model.UserId);
        ViewerFullScreen.SetValue(viewer, model.FullScreen);
        ViewerOnline.SetValue(viewer, model.Online);
        ViewerPause.SetValue(viewer, model.Pause);
        ViewerTimeLine.SetValue(viewer, model.TimeLine);
        ViewerName.SetValue(viewer, model.CustomName);
        ViewerVideoId.SetValue(viewer, model.VideoId);
        return viewer;
    }

    private static Message CreateMessage(MessageModel<YoutubeRoomModel> model)
    {
        var message = new Message(model.UserId, model.Text);
        MessageCreatedAt.SetValue(message, model.CreatedAt);
        return message;
    }

    private static Video CreateVideo(VideoModel model)
    {
        var video = (Video)RuntimeHelpers.GetUninitializedObject(VideoType);
        VideoId.SetValue(video, model.VideoId);
        VideoAdded.SetValue(video, model.Added);
        return video;
    }
}