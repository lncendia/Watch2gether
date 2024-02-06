using Microsoft.EntityFrameworkCore;
using Films.Domain.Abstractions.Repositories;
using Films.Domain.Ordering.Abstractions;
using Films.Domain.Servers.Entities;
using Films.Domain.Servers.Ordering.Visitor;
using Films.Domain.Servers.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Server;
using Films.Infrastructure.Storage.Visitors.Sorting;
using Films.Infrastructure.Storage.Visitors.Specifications;

namespace Films.Infrastructure.Storage.Repositories;

public class ServerRepository(
    ApplicationDbContext context,
    IAggregateMapperUnit<Server, ServerModel> aggregateMapper,
    IModelMapperUnit<ServerModel, Server> modelMapper)
    : IServerRepository
{
    public async Task AddAsync(Server entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        var room = await modelMapper.MapAsync(entity);
        await context.AddAsync(room);
    }

    public async Task UpdateAsync(Server entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        await modelMapper.MapAsync(entity);
    }

    public Task DeleteAsync(Guid id)
    {
        context.Remove(context.Servers.First(room => room.Id == id));
        return Task.CompletedTask;
    }

    public async Task<Server?> GetAsync(Guid id)
    {
        var room = await context.Servers.FirstOrDefaultAsync(youtubeServerModel => youtubeServerModel.Id == id);
        return room == null ? null : aggregateMapper.Map(room);
    }

    public async Task<IReadOnlyCollection<Server>> FindAsync(
        ISpecification<Server, IServerSpecificationVisitor>? specification,
        IOrderBy<Server, IServerSortingVisitor>? orderBy = null, int? skip = null,
        int? take = null)
    {
        var query = context.Servers.AsQueryable();
        if (specification != null)
        {
            var visitor = new ServerVisitor();
            specification.Accept(visitor);
            if (visitor.Expr != null) query = query.Where(visitor.Expr);
        }

        if (orderBy != null)
        {
            var visitor = new ServerSortingVisitor();
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

    public Task<int> CountAsync(ISpecification<Server, IServerSpecificationVisitor>? specification)
    {
        var query = context.Servers.AsQueryable();
        if (specification == null) return query.CountAsync();
        var visitor = new ServerVisitor();
        specification.Accept(visitor);
        if (visitor.Expr != null) query = query.Where(visitor.Expr);

        return query.CountAsync();
    }
}