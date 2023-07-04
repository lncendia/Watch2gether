using System.Reflection;
using Overoom.Domain.Rooms.BaseRoom.Entities;
using Overoom.Domain.Rooms.BaseRoom.ValueObject;

namespace Overoom.Infrastructure.Storage.Mappers.StaticMethods;

internal static class RoomInitializer
{
    private static readonly Type RoomType = typeof(Room);
    private static readonly Type MessageType = typeof(Message);

    private static readonly FieldInfo LastActivity =
        RoomType.GetField("<LastActivity>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo MessagesList =
        RoomType.GetField("MessagesList", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo ViewersList =
        RoomType.GetField("ViewersList", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo IdCounter =
        RoomType.GetField("IdCounter", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo Owner =
        RoomType.GetField("Owner", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo CreatedAt =
        MessageType.GetField("<CreatedAt>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    internal static void InitRoom(Room entity, DateTime lastActivity, int idCounter, IEnumerable<Viewer> viewers,
        int ownerId, IEnumerable<Message> messages)
    {
        LastActivity.SetValue(entity, lastActivity);
        IdCounter.SetValue(entity, idCounter);
        var viewersList = viewers.ToList();
        ViewersList.SetValue(entity, viewersList);
        Owner.SetValue(entity, viewersList.First(x => x.Id == ownerId));
        MessagesList.SetValue(entity, messages.ToList());
    }

    internal static Message CreateMessage(int entityId, string text, Guid roomId, DateTime createdAt)
    {
        object?[] args = { entityId, text, roomId };
        var element = (Message)MessageType.Assembly.CreateInstance(
            MessageType.FullName!, false, BindingFlags.Instance | BindingFlags.NonPublic, null, args!,
            null, null)!;
        CreatedAt.SetValue(element, createdAt);
        return element;
    }

    internal static void InitViewer(Viewer entity, bool online, bool onPause, TimeSpan timeLine)
    {
        entity.Online = online;
        entity.OnPause = onPause;
        entity.TimeLine = timeLine;
    }
}