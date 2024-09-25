using Microsoft.EntityFrameworkCore;
using Films.Domain.Abstractions.Repositories;
using Films.Domain.Ordering.Abstractions;
using Films.Domain.Rooms.FilmRooms;
using Films.Domain.Rooms.FilmRooms.Ordering.Visitor;
using Films.Domain.Rooms.FilmRooms.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.FilmRooms;
using Films.Infrastructure.Storage.Visitors.Sorting;
using Films.Infrastructure.Storage.Visitors.Specifications;

namespace Films.Infrastructure.Storage.Repositories;

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
            .Include(r => r.Viewers)
            .AsNoTracking()
            .FirstOrDefaultAsync(youtubeFilmRoomModel => youtubeFilmRoomModel.Id == id);

        return await (room == null ? null : aggregateMapper.MapAsync(room))!;
    }

    public async Task<IReadOnlyCollection<FilmRoom>> FindAsync(
        ISpecification<FilmRoom, IFilmRoomSpecificationVisitor>? specification,
        IOrderBy<FilmRoom, IFilmRoomSortingVisitor>? orderBy = null, int? skip = null,
        int? take = null)
    {
        var query = context.FilmRooms.AsQueryable();
        if (specification != null)
        {
            var visitor = new FilmRoomVisitor();
            specification.Accept(visitor);
            if (visitor.Expr != null) query = query.Where(visitor.Expr);
        }

        if (orderBy != null)
        {
            var visitor = new FilmRoomSortingVisitor();
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

        var entities = await query
            .Include(r => r.Viewers)
            .AsNoTracking()
            .ToArrayAsync();

        var aggregates = new FilmRoom[entities.Length];
        for (var i = 0; i < aggregates.Length; i++) aggregates[i] = await aggregateMapper.MapAsync(entities[i]);
        return aggregates;
    }

    public Task<int> CountAsync(ISpecification<FilmRoom, IFilmRoomSpecificationVisitor>? specification)
    {
        var query = context.FilmRooms.AsQueryable();
        if (specification == null) return query.CountAsync();
        var visitor = new FilmRoomVisitor();
        specification.Accept(visitor);
        if (visitor.Expr != null) query = query.Where(visitor.Expr);

        return query.CountAsync();
    }
}