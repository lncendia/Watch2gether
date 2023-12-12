using System.Reflection;
using Overoom.Domain.Rooms.BaseRoom.DTOs;
using Overoom.Domain.Rooms.FilmRoom.Entities;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;
using Overoom.Infrastructure.Storage.Models.FilmRoom;

namespace Overoom.Infrastructure.Storage.Mappers.AggregateMappers;

internal class FilmRoomMapper : IAggregateMapperUnit<FilmRoom, FilmRoomModel>
{
    private static readonly Type FilmViewerType = typeof(FilmViewer);
    private static readonly ViewerDto MockViewer = new("mock", new Uri("mock", UriKind.Relative));

    public FilmRoom Map(FilmRoomModel model)
    {
        var room = new FilmRoom(model.FilmId, model.CdnType, model.IsOpen, MockViewer);
        var viewers = model.Viewers.Select(CreateViewer);
        var messages = model.Messages.Select(x =>
            RoomInitializer.CreateMessage(x.Viewer.EntityId, x.Text, x.CreatedAt));
        RoomInitializer.InitRoom(room, model.Id, model.LastActivity, model.IdCounter, viewers, model.OwnerId, messages);
        return room;
    }

    private static FilmViewer CreateViewer(FilmViewerModel model)
    {
        var dto = RoomInitializer.CreateViewerDto(model.Name, model.AvatarUri, model.Beep, model.Scream, model.Change);
        object?[] args = { dto, model.EntityId, model.Season, model.Series };
        var element = (FilmViewer)FilmViewerType.Assembly.CreateInstance(
            FilmViewerType.FullName!, false, BindingFlags.Instance | BindingFlags.NonPublic, null, args!,
            null, null)!;
        RoomInitializer.InitViewer(element, model.Online, model.Pause, model.FullScreen, model.TimeLine);
        return element;
    }
}