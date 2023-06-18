using System.Reflection;
using Overoom.Domain.Rooms.BaseRoom.Entities;
using Overoom.Infrastructure.Storage.Models.Room.Base;

namespace Overoom.Infrastructure.Storage.Mappers.StaticMethods;

internal static class RoomModelInitializer
{
    private static readonly FieldInfo IdCounter =
        typeof(Room).GetField("IdCounter", BindingFlags.Instance | BindingFlags.NonPublic)!;

    internal static void InitRoomModel(RoomModel model, Room entity)
    {
        model.IsOpen = entity.IsOpen;
        model.LastActivity = entity.LastActivity;
        model.IdCounter = (int)IdCounter.GetValue(entity)!;

        var newMessages = entity.Messages.Where(x =>
            model.Messages.All(m => m.ViewerEntityId != x.ViewerId && m.CreatedAt != x.CreatedAt));
        model.Messages.AddRange(newMessages.Select(x => new MessageModel
            { ViewerEntityId = x.ViewerId, Text = x.Text, CreatedAt = x.CreatedAt }));
        model.Messages.RemoveAll(x =>
            entity.Messages.All(m => m.ViewerId != x.ViewerEntityId && m.CreatedAt != x.CreatedAt));

        //todo: check for delete
    }

    internal static void InitViewerModel(ViewerModel model, Viewer viewer)
    {
        model.Name = viewer.Name;
        model.AvatarUri = viewer.AvatarUri;
        model.Online = viewer.Online;
        model.OnPause = viewer.OnPause;
        model.TimeLine = viewer.TimeLine;
    }
}