using System.Reflection;
using Overoom.Domain.Rooms.BaseRoom.DTOs;
using Overoom.Domain.Rooms.BaseRoom.Entities;
using Overoom.Domain.Rooms.BaseRoom.ValueObjects;

namespace Overoom.Infrastructure.Storage.Mappers.StaticMethods;

internal static class RoomInitializer
{
    private static readonly Type RoomType = typeof(Room);
    private static readonly Type MessageType = typeof(Message);
    private static readonly Type ViewerDtoType = typeof(ViewerDto);

    private static readonly FieldInfo LastActivity =
        RoomType.GetField("<LastActivity>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo MessagesList =
        RoomType.GetField("_messagesList", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo ViewersList =
        RoomType.GetField("_viewersList", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo IdCounter =
        RoomType.GetField("_idCounter", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo Owner =
        RoomType.GetField("Owner", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo CreatedAt =
        MessageType.GetField("<CreatedAt>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo Beep =
        ViewerDtoType.GetField("<Beep>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;
    private static readonly FieldInfo Scream =
        ViewerDtoType.GetField("<Scream>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;
    private static readonly FieldInfo Change =
        ViewerDtoType.GetField("<Change>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    internal static void InitRoom(Room entity, Guid id, DateTime lastActivity, int idCounter,
        IEnumerable<Viewer> viewers, int ownerId, IEnumerable<Message> messages)
    {
        IdFields.AggregateId.SetValue(entity, id);
        LastActivity.SetValue(entity, lastActivity);
        IdCounter.SetValue(entity, idCounter);
        var viewersList = viewers.ToList();
        ViewersList.SetValue(entity, viewersList);
        Owner.SetValue(entity, viewersList.First(x => x.Id == ownerId));
        MessagesList.SetValue(entity, messages.ToList());
    }

    internal static Message CreateMessage(int entityId, string text, DateTime createdAt)
    {
        object?[] args = { entityId, text };
        var element = (Message)MessageType.Assembly.CreateInstance(MessageType.FullName!, false,
            BindingFlags.Instance | BindingFlags.NonPublic, null, args!, null, null)!;
        CreatedAt.SetValue(element, createdAt);
        return element;
    }

    internal static ViewerDto CreateViewerDto(string name, Uri avatar, bool beep, bool scream, bool change)
    {
        var dto = new ViewerDto(name, avatar);
        Beep.SetValue(dto, beep);
        Scream.SetValue(dto, scream);
        Change.SetValue(dto, change);
        return dto;

    }

    internal static void InitViewer(Viewer entity, bool online, bool pause, bool fullScreen, TimeSpan timeLine)
    {
        entity.Online = online;
        entity.Pause = pause;
        entity.TimeLine = timeLine;
        entity.FullScreen = fullScreen;
    }
}