using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Watch2gether.Domain.Abstractions.Repositories;
using Watch2gether.Domain.Ordering.Abstractions;
using Watch2gether.Domain.Rooms;
using Watch2gether.Domain.Specifications.Abstractions;
using Watch2gether.Domain.Rooms.Entities;
using Watch2gether.Domain.Rooms.Ordering.Visitor;
using Watch2gether.Domain.Rooms.Specifications.Visitor;
using Watch2gether.Domain.Rooms.ValueObject;
using Watch2gether.Infrastructure.PersistentStorage.Context;
using Watch2gether.Infrastructure.PersistentStorage.Models;
using Watch2gether.Infrastructure.PersistentStorage.Visitors.Sorting;
using Watch2gether.Infrastructure.PersistentStorage.Visitors.Specifications;

namespace Watch2gether.Infrastructure.PersistentStorage.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RoomRepository(ApplicationDbContext context)
    {
        _context = context;
        _mapper = GetMapper();
    }

    private Room GetMap(RoomModel model)
    {
        var room = new Room(model.FilmId, "someName", "someAvatar");
        _mapper.Map(model, room);
        var x = room.GetType();
        var viewersList = model.Viewers.Select(GetMap).OrderBy(viewer => viewer.Name).ToList();
        x.GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(room, model.Id);
        var viewers =
            (x.GetField("_viewers", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(room) as List<Viewer>)!;
        viewers.Clear();
        viewers.AddRange(viewersList);

        x.GetField("<Owner>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(room,
            viewersList.First(viewer => viewer.Id == model.OwnerId));

        var messages =
            (x.GetField("_messages", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(room) as List<Message>)!;
        messages.AddRange(model.Messages.Select(GetMap).OrderBy(message => message.CreatedAt));

        return room;
    }

    private Viewer GetMap(ViewerModel model)
    {
        var viewer = _mapper.Map<Viewer>(model);
        var x = viewer.GetType();
        x.GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(viewer, model.Id);
        return viewer;
    }

    private Message GetMap(MessageModel model)
    {
        var message = _mapper.Map<Message>(model);
        var x = message.GetType();
        x.GetField("<CreatedAt>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(message,
            model.CreatedAt);
        return message;
    }

    private RoomModel UpdateMap(Room room, RoomModel model)
    {
        var oldViewers = new List<ViewerModel>();
        var newViewers = new List<ViewerModel>();
        var viewers = room.Viewers;
        foreach (var viewer in viewers)
        {
            var viewerModel = model.Viewers.FirstOrDefault(v => v.Id == viewer.Id);
            if (viewerModel != null)
                oldViewers.Add(_mapper.Map(viewer, viewerModel));
            else
                newViewers.Add(_mapper.Map<ViewerModel>(viewer));
        }

        var oldMessages = new List<MessageModel>();
        var newMessages = new List<MessageModel>();
        var messages = room.Messages;
        foreach (var message in messages)
        {
            var messageModel = model.Messages.FirstOrDefault(v =>
                v.Text == message.Text && v.ViewerId == message.ViewerId && v.CreatedAt == message.CreatedAt);
            if (messageModel != null)
                oldMessages.Add(_mapper.Map(message, messageModel));
            else
                newMessages.Add(_mapper.Map(message, new MessageModel {Id = Guid.NewGuid()}));
        }

        _context.RemoveRange(model.Viewers.Where(x => !oldViewers.Contains(x)));
        _context.RemoveRange(model.Messages.Where(x => !oldMessages.Contains(x)));
        model.Viewers.Clear();
        model.Messages.Clear();

        _mapper.Map(room, model);

        _context.AddRange(newViewers);
        _context.AddRange(newMessages);

        model.Viewers.AddRange(oldViewers);
        model.Messages.AddRange(oldMessages);
        return model;
    }

    public async Task AddAsync(Room entity)
    {
        var room = new RoomModel();
        UpdateMap(entity, room);
        await _context.AddAsync(room);
    }

    public async Task AddRangeAsync(IList<Room> entities)
    {
        var rooms = entities.Select(x => UpdateMap(x, new RoomModel())).ToList();
        await _context.AddRangeAsync(rooms);
    }

    public async Task UpdateAsync(Room entity)
    {
        var model = await _context.Rooms.Include(x => x.Messages).Include(x => x.Viewers)
            .FirstAsync(x => x.Id == entity.Id);
        UpdateMap(entity, model);
    }

    public async Task UpdateRangeAsync(IList<Room> entities)
    {
        var ids = entities.Select(room => room.Id);
        var rooms = await _context.Rooms.Include(x => x.Messages).Include(x => x.Viewers)
            .Where(room => ids.Contains(room.Id)).ToListAsync();
        foreach (var entity in entities)
            UpdateMap(entity, rooms.First(roomModel => roomModel.Id == entity.Id));
    }

    public Task DeleteAsync(Room entity)
    {
        _context.Remove(_context.Rooms.Include(x => x.Messages).Include(x => x.Viewers)
            .First(room => room.Id == entity.Id));
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IList<Room> entities)
    {
        var ids = entities.Select(room => room.Id);
        _context.RemoveRange(_context.Rooms.Include(x => x.Messages).Include(x => x.Viewers)
            .Where(room => ids.Contains(room.Id)));
        return Task.CompletedTask;
    }

    public async Task<Room?> GetAsync(Guid id)
    {
        var room = await _context.Rooms.Include(x => x.Messages).Include(x => x.Viewers)
            .FirstOrDefaultAsync(roomModel => roomModel.Id == id);
        return room == null ? null : GetMap(room);
    }

    public async Task<IList<Room>> FindAsync(ISpecification<Room, IRoomSpecificationVisitor>? specification,
        IOrderBy<Room, IRoomSortingVisitor>? orderBy = null, int? skip = null,
        int? take = null)
    {
        var query = _context.Rooms.Include(x => x.Messages).Include(x => x.Viewers).AsQueryable();
        if (specification != null)
        {
            var visitor = new RoomVisitor();
            specification.Accept(visitor);
            if (visitor.Expr != null) query = query.Where(visitor.Expr);
        }
        
        if(orderBy != null)
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

        return (await query.ToListAsync()).Select(GetMap).ToList();
    }

    public Task<int> CountAsync(ISpecification<Room, IRoomSpecificationVisitor>? specification)
    {
        var query = _context.Rooms.Include(x => x.Messages).Include(x => x.Viewers).AsQueryable();
        if (specification == null) return query.CountAsync();
        var visitor = new RoomVisitor();
        specification.Accept(visitor);
        if (visitor.Expr != null) query = query.Where(visitor.Expr);

        return query.CountAsync();
    }

    private static IMapper GetMapper() => new Mapper(new MapperConfiguration(expr =>
    {
        expr.CreateMap<RoomModel, Room>().ForMember(x => x.Messages, opt => opt.Ignore())
            .ForMember(x => x.Viewers, opt => opt.Ignore());

        expr.CreateMap<Room, RoomModel>().ForMember(x => x.Messages, opt => opt.Ignore())
            .ForMember(x => x.Viewers, opt => opt.Ignore());

        expr.CreateMap<ViewerModel, Viewer>().ReverseMap();
        expr.CreateMap<MessageModel, Message>().ReverseMap();
    }));
}