using Films.Domain.Rooms.Entities;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Mappers.StaticMethods;
using Films.Infrastructure.Storage.Models.Room;

namespace Films.Infrastructure.Storage.Mappers.AggregateMappers;

internal class RoomMapper : IAggregateMapperUnit<Room, RoomModel>
{
    public Room Map(RoomModel model)
    {
        var room = new Room
        {
            IsOpen = model.IsOpen,
            OwnerId = model.OwnerId,
            ServerId = model.ServerId,
            Type = model.Type,
            ViewersCount = model.ViewersCount
        };
        IdFields.AggregateId.SetValue(room, model.Id);
        return room;
    }
}