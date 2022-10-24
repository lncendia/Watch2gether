using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Overoom.Domain.Abstractions.Repositories;
using Overoom.Infrastructure.PersistentStorage.Context;
using Overoom.Infrastructure.PersistentStorage.Models.Rooms;
using Overoom.Infrastructure.PersistentStorage.Visitors.Sorting;
using Overoom.Infrastructure.PersistentStorage.Visitors.Specifications;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Rooms.BaseRoom.Entities;
using Overoom.Domain.Rooms.BaseRoom.ValueObject;
using Overoom.Domain.Rooms.FilmRoom;
using Overoom.Domain.Rooms.FilmRoom.Entities;
using Overoom.Domain.Rooms.FilmRoom.Ordering.Visitor;
using Overoom.Domain.Rooms.FilmRoom.Specifications.Visitor;
using Overoom.Domain.Specifications.Abstractions;

namespace Overoom.Infrastructure.PersistentStorage.Repositories;

public class FilmRoomRepository : IFilmRoomRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public FilmRoomRepository(ApplicationDbContext context)
    {
        _context = context;
        _mapper = GetMapper();
    }

    private FilmRoom GetMap(FilmRoomModel model)
    {
        var room = new FilmRoom(model.FilmId, "someName", "someAvatar");
        _mapper.Map(model, room);
        var type = room.GetType();
        var btype = type.BaseType!;

        var viewersList = model.Viewers.Select(GetMap).OrderBy(viewer => viewer.Name).ToList();

        btype.GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(room, model.Id);
        var viewers =
            (btype.GetField("ViewersList", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(room) as
                List<Viewer>)!;
        viewers.Clear();
        viewers.AddRange(viewersList);

        type.GetField("<Owner>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(room,
            viewersList.First(viewer => viewer.Id == model.OwnerId));

        btype.GetField("<LastActivity>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(room,
            model.LastActivity);

        var messages =
            (btype.GetField("MessagesList", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(room) as
                List<Message>)!;
        messages.AddRange(model.Messages.Select(GetMap).OrderBy(message => message.CreatedAt));

        return room;
    }

    private FilmViewer GetMap(FilmViewerModel model)
    {
        var viewer = _mapper.Map<FilmViewer>(model);
        var x = viewer.GetType().BaseType!;
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

    private FilmRoomModel UpdateMap(FilmRoom room, FilmRoomModel model)
    {
        var oldViewers = new List<FilmViewerModel>();
        var newViewers = new List<FilmViewerModel>();
        var viewers = room.Viewers;
        foreach (var viewer in viewers)
        {
            var viewerModel = model.Viewers.FirstOrDefault(v => v.Id == viewer.Id);
            if (viewerModel != null)
                oldViewers.Add(_mapper.Map(viewer, viewerModel));
            else
                newViewers.Add(_mapper.Map<FilmViewerModel>(viewer));
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
                newMessages.Add(_mapper.Map<MessageModel>(message));
        }

        model.Viewers.RemoveAll(x => !oldViewers.Contains(x));
        model.Messages.RemoveAll(x => !oldMessages.Contains(x));
        // _context.RemoveRange(model.Viewers.Where(x => !oldViewers.Contains(x)));
        // _context.RemoveRange(model.Messages.Where(x => !oldMessages.Contains(x)));
        // model.Viewers.Clear();
        // model.Messages.Clear();

        _mapper.Map(room, model);

        _context.AddRange(newViewers);
        _context.AddRange(newMessages);

        // model.Viewers.AddRange(oldViewers);
        // model.Messages.AddRange(oldMessages);
        return model;
    }

    public async Task AddAsync(FilmRoom entity)
    {
        var room = new FilmRoomModel();
        UpdateMap(entity, room);
        await _context.AddAsync(room);
    }

    public async Task AddRangeAsync(IList<FilmRoom> entities)
    {
        var rooms = entities.Select(x => UpdateMap(x, new FilmRoomModel())).ToList();
        await _context.AddRangeAsync(rooms);
    }

    public async Task UpdateAsync(FilmRoom entity)
    {
        var model = await _context.FilmRooms.Include(x => x.Messages).Include(x => x.Viewers)
            .FirstAsync(x => x.Id == entity.Id);
        UpdateMap(entity, model);
        Console.WriteLine("film room updated");
    }

    public async Task UpdateRangeAsync(IList<FilmRoom> entities)
    {
        var ids = entities.Select(room => room.Id);
        var rooms = await _context.FilmRooms.Include(x => x.Messages).Include(x => x.Viewers)
            .Where(room => ids.Contains(room.Id)).ToListAsync();
        foreach (var entity in entities)
            UpdateMap(entity, rooms.First(filmRoomModel => filmRoomModel.Id == entity.Id));
    }

    public Task DeleteAsync(FilmRoom entity)
    {
        _context.Remove(_context.FilmRooms.First(room => room.Id == entity.Id));
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<FilmRoom> entities)
    {
        var ids = entities.Select(room => room.Id);
        _context.RemoveRange(_context.FilmRooms.Where(room => ids.Contains(room.Id)));
        return Task.CompletedTask;
    }

    public async Task<FilmRoom?> GetAsync(Guid id)
    {
        var room = await _context.FilmRooms.Include(x => x.Messages).Include(x => x.Viewers)
            .FirstOrDefaultAsync(filmRoomModel => filmRoomModel.Id == id);
        return room == null ? null : GetMap(room);
    }

    public async Task<IList<FilmRoom>> FindAsync(ISpecification<FilmRoom, IFilmRoomSpecificationVisitor>? specification,
        IOrderBy<FilmRoom, IFilmRoomSortingVisitor>? orderBy = null, int? skip = null,
        int? take = null)
    {
        var query = _context.FilmRooms.Include(x => x.Messages).Include(x => x.Viewers).AsQueryable();
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
            query = visitor.SortItems.Skip(1)
                .Aggregate(orderedQuery, (current, sort) => sort.IsDescending
                    ? current.ThenByDescending(sort.Expr)
                    : current.ThenBy(sort.Expr));
        }

        if (skip.HasValue) query = query.Skip(skip.Value);
        if (take.HasValue) query = query.Take(take.Value);

        return (await query.ToListAsync()).Select(GetMap).ToList();
    }

    public Task<int> CountAsync(ISpecification<FilmRoom, IFilmRoomSpecificationVisitor>? specification)
    {
        var query = _context.FilmRooms.Include(x => x.Messages).Include(x => x.Viewers).AsQueryable();
        if (specification == null) return query.CountAsync();
        var visitor = new FilmRoomVisitor();
        specification.Accept(visitor);
        if (visitor.Expr != null) query = query.Where(visitor.Expr);

        return query.CountAsync();
    }

    private static IMapper GetMapper() => new Mapper(new MapperConfiguration(expr =>
    {
        expr.CreateMap<FilmRoomModel, FilmRoom>().ForMember(x => x.Messages, opt => opt.Ignore())
            .ForMember(x => x.Viewers, opt => opt.Ignore());

        expr.CreateMap<FilmRoom, FilmRoomModel>().ForMember(x => x.Messages, opt => opt.Ignore())
            .ForMember(x => x.Viewers, opt => opt.Ignore());

        expr.CreateMap<FilmViewerModel, FilmViewer>().ReverseMap();
        expr.CreateMap<MessageModel, Message>().ReverseMap();
    }));
}