using Microsoft.EntityFrameworkCore;
using Room.Domain.Abstractions.Repositories;
using Room.Domain.Ordering.Abstractions;
using Room.Domain.Rooms.FilmRooms;
using Room.Domain.Rooms.FilmRooms.Ordering.Visitor;
using Room.Domain.Rooms.FilmRooms.Specifications.Visitor;
using Room.Domain.Specifications.Abstractions;
using Room.Infrastructure.Storage.Context;
using Room.Infrastructure.Storage.Extensions;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Models.FilmRooms;
using Room.Infrastructure.Storage.Visitors.Sorting;
using Room.Infrastructure.Storage.Visitors.Specifications;

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

        var models = await query            
            .LoadDependencies()
            .AsNoTracking()
            .ToArrayAsync();
        
        return models.Select(aggregateMapper.Map).ToArray();
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