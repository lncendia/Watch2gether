using Microsoft.EntityFrameworkCore;
using Room.Domain.Abstractions.Repositories;
using Room.Domain.YoutubeRooms;
using Room.Infrastructure.Storage.Context;
using Room.Infrastructure.Storage.Extensions;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Models.YoutubeRoom;

namespace Room.Infrastructure.Storage.Repositories;

public class YoutubeRoomRepository(
    ApplicationDbContext context,
    IAggregateMapperUnit<YoutubeRoom, YoutubeRoomModel> aggregateMapper,
    IModelMapperUnit<YoutubeRoomModel, YoutubeRoom> modelMapper)
    : IYoutubeRoomRepository
{
    public async Task AddAsync(YoutubeRoom entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        var room = await modelMapper.MapAsync(entity);
        await context.AddAsync(room);
    }

    public async Task UpdateAsync(YoutubeRoom entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        await modelMapper.MapAsync(entity);
    }

    public Task DeleteAsync(Guid id)
    {
        context.Remove(context.YoutubeRooms.First(room => room.Id == id));
        return Task.CompletedTask;
    }

    public async Task<YoutubeRoom?> GetAsync(Guid id)
    {
        var room = await context.YoutubeRooms
            .LoadDependencies()
            .AsNoTracking()
            .FirstOrDefaultAsync(youtubeRoomModel => youtubeRoomModel.Id == id);
        
        return room == null ? null : aggregateMapper.Map(room);
    }
}