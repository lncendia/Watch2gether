using System.Reflection;
using Overoom.Domain.Room.BaseRoom.Entities;
using Overoom.Domain.Room.BaseRoom.ValueObject;
using Overoom.Infrastructure.Storage.Models.Room.Base;

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

    internal static void InitRoom(Room entity, RoomModel model, IEnumerable<Viewer> viewers, int ownerId)
    {
        LastActivity.SetValue(entity, model.LastActivity);
        IdCounter.SetValue(entity, model.IdCounter);
        var viewersList = viewers.ToList();
        ViewersList.SetValue(entity, viewersList);
        Owner.SetValue(entity, viewersList.First(x => x.Id == ownerId));
        var messages = model.Messages.Select(GetMessage).ToList();
        MessagesList.SetValue(entity, messages);
    }

    private static Message GetMessage(MessageModel model)
    {
        object?[] args = { model.ViewerEntityId, model.Text, model.RoomId };
        var element = (Message)MessageType.Assembly.CreateInstance(
            MessageType.FullName!, false, BindingFlags.Instance | BindingFlags.NonPublic, null, args!,
            null, null)!;
        CreatedAt.SetValue(element, CreatedAt);
        return element;
    }

    internal static void InitViewer(Viewer entity, ViewerModel model)
    {
        entity.Online = model.Online;
        entity.OnPause = model.OnPause;
        entity.TimeLine = model.TimeLine;
    }
}