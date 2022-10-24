using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Overoom.Domain.Abstractions.Repositories;
using Overoom.Infrastructure.PersistentStorage.Context;
using Overoom.Infrastructure.PersistentStorage.Models.Playlists;
using Overoom.Infrastructure.PersistentStorage.Visitors.Sorting;
using Overoom.Infrastructure.PersistentStorage.Visitors.Specifications;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.Playlists;
using Overoom.Domain.Playlists.Ordering.Visitor;
using Overoom.Domain.Playlists.Specifications.Visitor;
using Overoom.Infrastructure.PersistentStorage.Models;

namespace Overoom.Infrastructure.PersistentStorage.Repositories;

public class PlaylistRepository : IPlaylistRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PlaylistRepository(ApplicationDbContext context)
    {
        _context = context;
        _mapper = GetMapper();
    }

    private Playlist Map(PlaylistModel model)
    {
        var playlist = _mapper.Map<Playlist>(model);
        var x = playlist.GetType();
        x.GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(playlist, model.Id);
        x.GetField("<Updated>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(playlist,
            model.Updated);
        return playlist;
    }

    public async Task AddAsync(Playlist entity)
    {
        var playlist = _mapper.Map<Playlist, PlaylistModel>(entity);
        await _context.AddAsync(playlist);
    }

    public async Task AddRangeAsync(IList<Playlist> entities)
    {
        var playlists = _mapper.Map<IList<Playlist>, List<PlaylistModel>>(entities);
        await _context.AddRangeAsync(playlists);
    }

    public async Task UpdateAsync(Playlist entity)
    {
        var model = await _context.Playlists.FirstAsync(x => x.Id == entity.Id);
        _mapper.Map(entity, model);
    }

    public async Task UpdateRangeAsync(IList<Playlist> entities)
    {
        var ids = entities.Select(playlist => playlist.Id);
        var playlists = await _context.Playlists.Where(playlist => ids.Contains(playlist.Id)).ToListAsync();
        foreach (var entity in entities)
            _mapper.Map(entity, playlists.First(playlistModel => playlistModel.Id == entity.Id));
    }

    public Task DeleteAsync(Playlist entity)
    {
        _context.Remove(_context.Playlists.First(playlist => playlist.Id == entity.Id));
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<Playlist> entities)
    {
        var ids = entities.Select(playlist => playlist.Id);
        _context.RemoveRange(_context.Playlists.Where(playlist => ids.Contains(playlist.Id)));
        return Task.CompletedTask;
    }

    public async Task<Playlist?> GetAsync(Guid id)
    {
        var playlist = await _context.Playlists.FirstOrDefaultAsync(playlistModel => playlistModel.Id == id);
        return playlist == null ? null : Map(playlist);
    }

    public async Task<IList<Playlist>> FindAsync(ISpecification<Playlist, IPlaylistSpecificationVisitor>? specification,
        IOrderBy<Playlist, IPlaylistSortingVisitor>? orderBy = null, int? skip = null, int? take = null)
    {
        var query = _context.Playlists.AsQueryable();
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

        return (await query.ToListAsync()).Select(Map).ToList();
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

    private static IMapper GetMapper() => new Mapper(new MapperConfiguration(expr =>
    {
        expr.CreateMap<Playlist, PlaylistModel>().ReverseMap();
    }));
}