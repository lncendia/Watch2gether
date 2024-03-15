using Films.Domain.Abstractions.Repositories;
using Films.Domain.Ordering.Abstractions;
using Films.Domain.Rooms.YoutubeRooms;
using Films.Domain.Rooms.YoutubeRooms.Ordering.Visitor;
using Films.Domain.Rooms.YoutubeRooms.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.YoutubeRoom;
using Films.Infrastructure.Storage.Visitors.Sorting;
using Films.Infrastructure.Storage.Visitors.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Storage.Repositories;

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
            .Include(r=>r.Viewers)
            .AsNoTracking()
            .FirstOrDefaultAsync(youtubeYoutubeRoomModel => youtubeYoutubeRoomModel.Id == id);
        
        return room == null ? null : aggregateMapper.Map(room);
    }

    public async Task<IReadOnlyCollection<YoutubeRoom>> FindAsync(
        ISpecification<YoutubeRoom, IYoutubeRoomSpecificationVisitor>? specification,
        IOrderBy<YoutubeRoom, IYoutubeRoomSortingVisitor>? orderBy = null, int? skip = null,
        int? take = null)
    {
        var query = context.YoutubeRooms.AsQueryable();
        if (specification != null)
        {
            var visitor = new YoutubeRoomVisitor();
            specification.Accept(visitor);
            if (visitor.Expr != null) query = query.Where(visitor.Expr);
        }

        if (orderBy != null)
        {
            var visitor = new YoutubeRoomSortingVisitor();
            orderBy.Accept(visitor);
            var firstQuery = visitor.SortItems.First();
            var orderedQuery = firstQuery.IsDescending
                ? query.OrderByDescending(firstQuery.Expr)
                : query.OrderBy(firstQuery.Expr);

            orderedQuery = visitor.SortItems.Skip(1)
                .Aggregate(orderedQuery, (current, sort) => sort.IsDescending
                    ? current.ThenByDescending(sort.Expr)
                    : current.ThenBy(sort.Expr));
            
            query = orderedQuery.ThenBy(v => v.Id);
        }
        else
        {
            query = query.OrderBy(x => x.Id);
        }

        if (skip.HasValue) query = query.Skip(skip.Value);
        if (take.HasValue) query = query.Take(take.Value);

        var models = await query            
            .Include(r=>r.Viewers)
            .AsNoTracking()
            .ToArrayAsync();
        
        return models.Select(aggregateMapper.Map).ToArray();
    }

    public Task<int> CountAsync(ISpecification<YoutubeRoom, IYoutubeRoomSpecificationVisitor>? specification)
    {
        var query = context.YoutubeRooms.AsQueryable();
        if (specification == null) return query.CountAsync();
        var visitor = new YoutubeRoomVisitor();
        specification.Accept(visitor);
        if (visitor.Expr != null) query = query.Where(visitor.Expr);

        return query.CountAsync();
    }
}