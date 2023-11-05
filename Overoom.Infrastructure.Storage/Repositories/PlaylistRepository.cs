using Microsoft.EntityFrameworkCore;
using Overoom.Domain.Abstractions.Repositories;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Playlists.Entities;
using Overoom.Domain.Playlists.Ordering.Visitor;
using Overoom.Domain.Playlists.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Infrastructure.Storage.Context;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Models.Playlist;
using Overoom.Infrastructure.Storage.Visitors.Sorting;
using Overoom.Infrastructure.Storage.Visitors.Specifications;

namespace Overoom.Infrastructure.Storage.Repositories;

public class PlaylistRepository : IPlaylistRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IAggregateMapperUnit<Playlist, PlaylistModel> _aggregateMapper;
    private readonly IModelMapperUnit<PlaylistModel, Playlist> _modelMapper;

    public PlaylistRepository(ApplicationDbContext context,
        IAggregateMapperUnit<Playlist, PlaylistModel> aggregateMapper,
        IModelMapperUnit<PlaylistModel, Playlist> modelMapper)
    {
        _context = context;
        _aggregateMapper = aggregateMapper;
        _modelMapper = modelMapper;
    }

    public async Task AddAsync(Playlist entity)
    {
        _context.Notifications.AddRange(entity.DomainEvents);
        var playlist = await _modelMapper.MapAsync(entity);
        await _context.AddAsync(playlist);
    }

    public async Task UpdateAsync(Playlist entity)
    {
        _context.Notifications.AddRange(entity.DomainEvents);
        await _modelMapper.MapAsync(entity);
    }
    

    public Task DeleteAsync(Guid id)
    {
        _context.Remove(_context.Playlists.First(playlist => playlist.Id == id));
        return Task.CompletedTask;
    }

    public async Task<Playlist?> GetAsync(Guid id)
    {
        var playlist = await _context.Playlists.FirstOrDefaultAsync(playlistModel => playlistModel.Id == id);
        if (playlist == null) return null;
        await LoadCollectionsAsync(playlist);
        return _aggregateMapper.Map(playlist);
    }

    public async Task<IList<Playlist>> FindAsync(ISpecification<Playlist, IPlaylistSpecificationVisitor>? specification,
        IOrderBy<Playlist, IPlaylistSortingVisitor>? orderBy = null, int? skip = null, int? take = null)
    {
        var query = _context.Playlists.Include(x=>x.Films).AsQueryable();
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
            query = visitor.SortItems.Skip(1)
                .Aggregate(orderedQuery, (current, sort) => sort.IsDescending
                    ? current.ThenByDescending(sort.Expr)
                    : current.ThenBy(sort.Expr));
        }

        if (skip.HasValue) query = query.Skip(skip.Value);
        if (take.HasValue) query = query.Take(take.Value);
        
        var models = await query.ToListAsync();
        foreach (var model in models) await LoadCollectionsAsync(model);
        return models.Select(_aggregateMapper.Map).ToList();
    }

    public Task<int> CountAsync(ISpecification<Playlist, IPlaylistSpecificationVisitor>? specification)
    {
        var query = _context.Playlists.AsQueryable();
        if (specification == null) return query.CountAsync();
        var visitor = new PlaylistVisitor();
        specification.Accept(visitor);
        if (visitor.Expr != null) query = query.Where(visitor.Expr);

        return query.CountAsync();
    }
    
    private async Task LoadCollectionsAsync(PlaylistModel model)
    {
        await _context.Entry(model).Collection(x => x.Films).LoadAsync();
        await _context.Entry(model).Collection(x => x.Genres).LoadAsync();
    }
}