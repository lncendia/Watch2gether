using Microsoft.EntityFrameworkCore;
using Films.Domain.Abstractions.Repositories;
using Films.Domain.Ordering.Abstractions;
using Films.Domain.Playlists;
using Films.Domain.Playlists.Ordering.Visitor;
using Films.Domain.Playlists.Specifications.Visitor;
using Films.Domain.Specifications.Abstractions;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Extensions;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Playlists;
using Films.Infrastructure.Storage.Visitors.Sorting;
using Films.Infrastructure.Storage.Visitors.Specifications;

namespace Films.Infrastructure.Storage.Repositories;

public class PlaylistRepository(
    ApplicationDbContext context,
    IAggregateMapperUnit<Playlist, PlaylistModel> aggregateMapper,
    IModelMapperUnit<PlaylistModel, Playlist> modelMapper)
    : IPlaylistRepository
{
    public async Task AddAsync(Playlist entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        var playlist = await modelMapper.MapAsync(entity);
        await context.AddAsync(playlist);
    }

    public async Task UpdateAsync(Playlist entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        await modelMapper.MapAsync(entity);
    }
    

    public Task DeleteAsync(Guid id)
    {
        context.Remove(context.Playlists.First(playlist => playlist.Id == id));
        return Task.CompletedTask;
    }

    public async Task<Playlist?> GetAsync(Guid id)
    {
        var playlist = await context.Playlists
            .LoadDependencies()
            .AsNoTracking()
            .FirstOrDefaultAsync(playlistModel => playlistModel.Id == id);
        
        return playlist == null ? null : aggregateMapper.Map(playlist);
    }

    public async Task<IReadOnlyCollection<Playlist>> FindAsync(
        ISpecification<Playlist, IPlaylistSpecificationVisitor>? specification,
        IOrderBy<Playlist, IPlaylistSortingVisitor>? orderBy = null, int? skip = null, int? take = null)
    {
        var query = context.Playlists.LoadDependencies().AsQueryable();
        if (specification != null)
        {
            var visitor = new PlaylistVisitor();
            specification.Accept(visitor);
            if (visitor.Expr != null) query = query.Where(visitor.Expr);
        }

        if (orderBy != null)
        {
            var visitor = new PlaylistSortingVisitor();
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

    public Task<int> CountAsync(ISpecification<Playlist, IPlaylistSpecificationVisitor>? specification)
    {
        var query = context.Playlists.AsQueryable();
        if (specification == null) return query.CountAsync();
        var visitor = new PlaylistVisitor();
        specification.Accept(visitor);
        if (visitor.Expr != null) query = query.Where(visitor.Expr);

        return query.CountAsync();
    }
}