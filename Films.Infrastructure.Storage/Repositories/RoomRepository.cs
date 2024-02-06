using Microsoft.EntityFrameworkCore;
using Films.Domain.Abstractions.Repositories;
using Films.Domain.Ordering.Abstractions;
using Films.Domain.Rooms.Entities;
using Films.Domain.Rooms.Ordering.Visitor;
using Films.Domain.Rooms.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Room;
using Films.Infrastructure.Storage.Visitors.Sorting;
using Films.Infrastructure.Storage.Visitors.Specifications;

namespace Films.Infrastructure.Storage.Repositories;

public class RoomRepository(
    ApplicationDbContext context,
    IAggregateMapperUnit<Room, RoomModel> aggregateMapper,
    IModelMapperUnit<RoomModel, Room> modelMapper)
    : IRoomRepository
{
    public async Task AddAsync(Room entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        var room = await modelMapper.MapAsync(entity);
        await context.AddAsync(room);
    }

    public async Task UpdateAsync(Room entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        await modelMapper.MapAsync(entity);
    }

    public Task DeleteAsync(Guid id)
    {
        context.Remove(context.Rooms.First(room => room.Id == id));
        return Task.CompletedTask;
    }

    public async Task<Room?> GetAsync(Guid id)
    {
        var room = await context.Rooms.FirstOrDefaultAsync(youtubeRoomModel => youtubeRoomModel.Id == id);
        return room == null ? null : aggregateMapper.Map(room);
    }

    public async Task<IReadOnlyCollection<Room>> FindAsync(
        ISpecification<Room, IRoomSpecificationVisitor>? specification,
        IOrderBy<Room, IRoomSortingVisitor>? orderBy = null, int? skip = null,
        int? take = null)
    {
        var query = context.Rooms.AsQueryable();
        if (specification != null)
        {
            var visitor = new RoomVisitor();
            specification.Accept(visitor);
            if (visitor.Expr != null) query = query.Where(visitor.Expr);
        }

        if (orderBy != null)
        {
            var visitor = new RoomSortingVisitor();
            orderBy.Accept(visitor);
            var firstQuery = visitor.SortItems.First();
            var orderedQuery = firstQuery.IsDescending
                ? query.OrderByDescending(firstQuery.Expr)
                : query.OrderBy(firstQuery.Expr);
            query = visitor.SortItems.Skip(1)
                .Aggregate(orderedQuery, (current, sort) => sort.IsDescending
                    ? current.ThenByDescending(sort.Expr)
                    : current.ThenBy(sort.Expr));
        }

        if (skip.HasValue) query = query.Skip(skip.Value);
        if (take.HasValue) query = query.Take(take.Value);

        var models = await query.ToArrayAsync();
        return models.Select(aggregateMapper.Map).ToArray();
    }

    public Task<int> CountAsync(ISpecification<Room, IRoomSpecificationVisitor>? specification)
    {
        var query = context.Rooms.AsQueryable();
        if (specification == null) return query.CountAsync();
        var visitor = new RoomVisitor();
        specification.Accept(visitor);
        if (visitor.Expr != null) query = query.Where(visitor.Expr);

        return query.CountAsync();
    }
}