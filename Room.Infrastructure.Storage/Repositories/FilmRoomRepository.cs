using Microsoft.EntityFrameworkCore;
using Room.Domain.Abstractions.Repositories;
using Room.Domain.Rooms.FilmRoom.Entities;
using Room.Infrastructure.Storage.Context;
using Room.Infrastructure.Storage.Extensions;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Models.Room.FilmRoom;

namespace Room.Infrastructure.Storage.Repositories;

public class FilmRoomRepository(
    ApplicationDbContext context,
    IAggregateMapperUnit<FilmRoom, FilmRoomModel> aggregateMapper,
    IModelMapperUnit<FilmRoomModel, FilmRoom> modelMapper)
    : IFilmRoomRepository
{
    public async Task AddAsync(FilmRoom entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        var room = await modelMapper.MapAsync(entity);
        await context.AddAsync(room);
    }

    public async Task UpdateAsync(FilmRoom entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        await modelMapper.MapAsync(entity);
    }

    public Task DeleteAsync(Guid id)
    {
        context.Remove(context.FilmRooms.First(room => room.Id == id));
        return Task.CompletedTask;
    }

    public async Task<FilmRoom?> GetAsync(Guid id)
    {
        var room = await context.FilmRooms
            .LoadDependencies()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
        
        return room == null ? null : aggregateMapper.Map(room);
    }
}