using System.Reflection;
using System.Runtime.CompilerServices;
using Films.Domain.Abstractions;
using Films.Domain.Rooms.FilmRooms;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Mappers.StaticMethods;
using Films.Infrastructure.Storage.Models.FilmRooms;

namespace Films.Infrastructure.Storage.Mappers.AggregateMappers;

internal class FilmRoomMapper : IAggregateMapperUnit<FilmRoom, FilmRoomModel>
{
    private static readonly Type FilmRoomType = typeof(FilmRoom);

    private static readonly FieldInfo FilmId =
        FilmRoomType.GetField("<FilmId>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly FieldInfo CdnName =
        FilmRoomType.GetField("<CdnName>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public FilmRoom Map(FilmRoomModel model)
    {
        var room = (FilmRoom)RuntimeHelpers.GetUninitializedObject(FilmRoomType);
        FilmId.SetValue(room, model.FilmId);
        CdnName.SetValue(room, model.CdnName);
        RoomFields.Code.SetValue(room, model.Code);
        RoomFields.ServerId.SetValue(room, model.ServerId);
        RoomFields.Viewers.SetValue(room, model.Viewers.Select(v => v.UserId).ToList());
        RoomFields.Banned.SetValue(room, model.Banned.Select(b => b.UserId).ToList());
        IdFields.AggregateId.SetValue(room, model.Id);
        IdFields.DomainEvents.SetValue(room, new List<IDomainEvent>());

        return room;
    }
}