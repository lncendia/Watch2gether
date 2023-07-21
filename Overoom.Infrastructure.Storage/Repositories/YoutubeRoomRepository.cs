using Microsoft.EntityFrameworkCore;
using Overoom.Domain.Abstractions.Repositories;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Rooms.YoutubeRoom.Entities;
using Overoom.Domain.Rooms.YoutubeRoom.Ordering.Visitor;
using Overoom.Domain.Rooms.YoutubeRoom.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;
using Overoom.Infrastructure.Storage.Context;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Models.YoutubeRoom;
using Overoom.Infrastructure.Storage.Visitors.Sorting;
using Overoom.Infrastructure.Storage.Visitors.Specifications;

namespace Overoom.Infrastructure.Storage.Repositories;

public class YoutubeRoomRepository : IYoutubeRoomRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IAggregateMapperUnit<YoutubeRoom, YoutubeRoomModel> _aggregateMapper;
    private readonly IModelMapperUnit<YoutubeRoomModel, YoutubeRoom> _modelMapper;

    public YoutubeRoomRepository(ApplicationDbContext context,
        IAggregateMapperUnit<YoutubeRoom, YoutubeRoomModel> aggregateMapper,
        IModelMapperUnit<YoutubeRoomModel, YoutubeRoom> modelMapper)
    {
        _context = context;
        _aggregateMapper = aggregateMapper;
        _modelMapper = modelMapper;
    }

    public async Task AddAsync(YoutubeRoom entity)
    {
        _context.Notifications.AddRange(entity.DomainEvents);
        var room = await _modelMapper.MapAsync(entity);
        await _context.AddAsync(room);
    }

    public async Task UpdateAsync(YoutubeRoom entity)
    {
        _context.Notifications.AddRange(entity.DomainEvents);
        await _modelMapper.MapAsync(entity);
    }

    public Task DeleteAsync(Guid id)
    {
        _context.Remove(_context.YoutubeRooms.First(room => room.Id == id));
        return Task.CompletedTask;
    }

    public async Task<YoutubeRoom?> GetAsync(Guid id)
    {
        var room = await _context.YoutubeRooms.FirstOrDefaultAsync(youtubeRoomModel => youtubeRoomModel.Id == id);
        if (room == null) return null;
        await LoadCollectionsAsync(room);
        return _aggregateMapper.Map(room);
    }

    public async Task<IList<YoutubeRoom>> FindAsync(
        ISpecification<YoutubeRoom, IYoutubeRoomSpecificationVisitor>? specification,
        IOrderBy<YoutubeRoom, IYoutubeRoomSortingVisitor>? orderBy = null, int? skip = null,
        int? take = null)
    {
        var query = _context.YoutubeRooms.Include(x => x.VideoIds).Include(x => x.Messages).Include(x => x.Viewers)
            .AsQueryable();
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

    public Task<int> CountAsync(ISpecification<YoutubeRoom, IYoutubeRoomSpecificationVisitor>? specification)
    {
        var query = _context.YoutubeRooms.Include(x => x.Messages).Include(x => x.Viewers).AsQueryable();
        if (specification == null) return query.CountAsync();
        var visitor = new YoutubeRoomVisitor();
        specification.Accept(visitor);
        if (visitor.Expr != null) query = query.Where(visitor.Expr);

        return query.CountAsync();
    }

    private async Task LoadCollectionsAsync(YoutubeRoomModel model)
    {
        await _context.Entry(model).Collection(x => x.VideoIds).LoadAsync();
        await _context.Entry(model).Collection(x => x.Viewers).LoadAsync();
        await _context.Entry(model).Collection(x => x.Messages).LoadAsync();
    }
}