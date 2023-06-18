using System.Reflection;
using Overoom.Domain.Rooms.FilmRoom.Entities;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;
using Overoom.Infrastructure.Storage.Models.Room.FilmRoom;

namespace Overoom.Infrastructure.Storage.Mappers.AggregateMappers;

internal class FilmRoomMapper : IAggregateMapperUnit<FilmRoom, FilmRoomModel>
{
    private static readonly Type FilmRoomType = typeof(FilmRoom);
    private static readonly Type FilmViewerType = typeof(FilmViewer);
    

    public FilmRoom Map(FilmRoomModel model)
    {
        var room = new FilmRoom(model.FilmId, "mockName", "mockUri", model.CdnType);
        var viewers = model.Viewers.Cast<FilmViewerModel>().Select(CreateViewer);
        RoomInitializer.InitRoom(room, model, viewers, model.OwnerId);
        return room;
    }

    private static FilmViewer CreateViewer(FilmViewerModel model)
    {
        object?[] args = { model.EntityId, model.Name, model.AvatarUri, model.Season, model.Series };
        var element = (FilmViewer)FilmViewerType.Assembly.CreateInstance(
            FilmViewerType.FullName!, false, BindingFlags.Instance | BindingFlags.NonPublic, null, args!,
            null, null)!;
        RoomInitializer.InitViewer(element, model);
        return element;
    }
}