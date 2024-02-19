using System.Reflection;
using System.Runtime.CompilerServices;
using Films.Domain.Abstractions;
using Films.Domain.Rooms.YoutubeRooms;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Mappers.StaticMethods;
using Films.Infrastructure.Storage.Models.Rooms.YoutubeRoom;

namespace Films.Infrastructure.Storage.Mappers.AggregateMappers;

internal class YoutubeRoomMapper : IAggregateMapperUnit<YoutubeRoom, YoutubeRoomModel>
{
    private static readonly Type YoutubeRoomType = typeof(YoutubeRoom);

    private static readonly FieldInfo VideoAccess =
        YoutubeRoomType.GetField("<VideoAccess>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;
    
    public YoutubeRoom Map(YoutubeRoomModel model)
    {
        var room = (YoutubeRoom)RuntimeHelpers.GetUninitializedObject(YoutubeRoomType);
        VideoAccess.SetValue(room, model.VideoAccess);
        RoomFields.Code.SetValue(room, model.Code);
        RoomFields.ServerId.SetValue(room, model.ServerId);
        RoomFields.Viewers.SetValue(room, model.Viewers.Select(v => v.UserId).ToList());
        RoomFields.Banned.SetValue(room, model.Banned.Select(b=>b.UserId).ToList());
        IdFields.AggregateId.SetValue(room, model.Id);
        IdFields.DomainEvents.SetValue(room, new List<IDomainEvent>());

        return room;
    }
}