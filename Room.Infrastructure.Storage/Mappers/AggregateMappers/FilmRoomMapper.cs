using System.Reflection;
using System.Runtime.CompilerServices;
using Room.Domain.Abstractions;
using Room.Domain.Rooms.BaseRoom.Entities;
using Room.Domain.Rooms.BaseRoom.ValueObjects;
using Room.Domain.Rooms.FilmRoom.Entities;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Mappers.StaticMethods;
using Room.Infrastructure.Storage.Models.Room.Base;
using Room.Infrastructure.Storage.Models.Room.FilmRoom;

namespace Room.Infrastructure.Storage.Mappers.AggregateMappers;

internal class FilmRoomMapper : IAggregateMapperUnit<FilmRoom, FilmRoomModel>
{
    private static readonly Type FilmViewerType = typeof(FilmViewer);
    private static readonly Type ViewerType = typeof(Viewer);

    private static readonly Type FilmRoomType = typeof(FilmRoom);
    private static readonly Type RoomType = typeof(Room<FilmViewer>);

    private static readonly FieldInfo FilmId =
        FilmRoomType.GetField("<FilmId>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo CdnName =
        FilmRoomType.GetField("<CdnName>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

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

    private static readonly FieldInfo ViewerSeason =
        FilmViewerType.GetField("<Season>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo ViewerSeries =
        FilmViewerType.GetField("<Series>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo MessageCreatedAt =
        typeof(Message).GetField("<CreatedAt>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public FilmRoom Map(FilmRoomModel model)
    {
        var room = (FilmRoom)RuntimeHelpers.GetUninitializedObject(FilmRoomType);
        var viewers = model.Viewers.Select(CreateViewer).ToList();
        var messages = model.Messages.Select(CreateMessage).ToList();
        var bannedList = model.BannedUsers.Select(b => b.UserId).ToList();

        var owner = viewers.First(v => v.UserId == model.Viewers.First(m => m.Owner).UserId);

        FilmId.SetValue(room, model.FilmId);
        CdnName.SetValue(room, model.CdnName);
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

    private static FilmViewer CreateViewer(FilmViewerModel model)
    {
        var viewer = (FilmViewer)RuntimeHelpers.GetUninitializedObject(FilmViewerType);
        ViewerUserId.SetValue(viewer, model.UserId);
        ViewerFullScreen.SetValue(viewer, model.FullScreen);
        ViewerOnline.SetValue(viewer, model.Online);
        ViewerPause.SetValue(viewer, model.Pause);
        ViewerTimeLine.SetValue(viewer, model.TimeLine);
        ViewerName.SetValue(viewer, model.CustomName);
        ViewerSeason.SetValue(viewer, model.Season);
        ViewerSeries.SetValue(viewer, model.Series);
        return viewer;
    }

    private static Message CreateMessage(MessageModel<FilmRoomModel> model)
    {
        var message = new Message(model.UserId, model.Text);
        MessageCreatedAt.SetValue(message, model.CreatedAt);
        return message;
    }
}