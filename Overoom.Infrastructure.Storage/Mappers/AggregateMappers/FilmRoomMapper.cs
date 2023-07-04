using System.Reflection;
using Overoom.Domain.Rooms.FilmRoom.Entities;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;
using Overoom.Infrastructure.Storage.Models.FilmRoom;

namespace Overoom.Infrastructure.Storage.Mappers.AggregateMappers;

internal class FilmRoomMapper : IAggregateMapperUnit<FilmRoom, FilmRoomModel>
{
    private static readonly Type FilmViewerType = typeof(FilmViewer);
    private static readonly Uri MockUri = new("mock", UriKind.Relative);

    public FilmRoom Map(FilmRoomModel model)
    {
        var room = new FilmRoom(model.FilmId, "mockName", MockUri, model.CdnType);
        var viewers = model.Viewers.Select(CreateViewer);
        var messages = model.Messages.Select(x =>
            RoomInitializer.CreateMessage(x.ViewerEntityId, x.Text, x.RoomId, x.CreatedAt));
        RoomInitializer.InitRoom(room, model.LastActivity, model.IdCounter, viewers, model.OwnerId, messages);
        return room;
    }

    private static FilmViewer CreateViewer(FilmViewerModel model)
    {
        object?[] args = { model.EntityId, model.Name, model.AvatarUri, model.Season, model.Series };
        var element = (FilmViewer)FilmViewerType.Assembly.CreateInstance(
            FilmViewerType.FullName!, false, BindingFlags.Instance | BindingFlags.NonPublic, null, args!,
            null, null)!;
        RoomInitializer.InitViewer(element, model.Online, model.OnPause, model.TimeLine);
        return element;
    }
}