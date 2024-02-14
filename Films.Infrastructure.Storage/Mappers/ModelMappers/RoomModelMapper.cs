using Microsoft.EntityFrameworkCore;
using Films.Domain.Rooms.Entities;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Room;

namespace Films.Infrastructure.Storage.Mappers.ModelMappers;

internal class RoomModelMapper(ApplicationDbContext context) : IModelMapperUnit<RoomModel, Room>
{
    public async Task<RoomModel> MapAsync(Room entity)
    {
        var room = await context.Rooms.FirstOrDefaultAsync(x => x.Id == entity.Id) ?? new RoomModel
        {
            Id = entity.Id,
            OwnerId = entity.OwnerId,
            ServerId = entity.ServerId,
            Type = entity.Type
        };

        room.IsOpen = entity.IsOpen;
        room.ViewersCount = entity.ViewersCount;
        return room;
    }
}