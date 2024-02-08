using Films.Domain.Abstractions.Interfaces;
using MediatR;
using Films.Domain.Abstractions.Repositories;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.AggregateMappers;
using Films.Infrastructure.Storage.Mappers.ModelMappers;
using Films.Infrastructure.Storage.Repositories;

namespace Films.Infrastructure.Storage;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IMediator _mediator;

    public UnitOfWork(ApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
        UserRepository = new Lazy<IUserRepository>(() =>
            new UserRepository(_context, new UserMapper(), new UserModelMapper(_context)));
        FilmRepository = new Lazy<IFilmRepository>(() =>
            new FilmRepository(_context, new FilmMapper(), new FilmModelMapper(_context)));
        ServerRepository = new Lazy<IServerRepository>(() =>
            new ServerRepository(_context, new ServerMapper(), new ServerModelMapper(_context)));
        RoomRepository = new Lazy<IRoomRepository>(() =>
            new RoomRepository(_context, new RoomMapper(), new RoomModelMapper(_context)));
        PlaylistRepository = new Lazy<IPlaylistRepository>(() =>
            new PlaylistRepository(_context, new PlaylistMapper(), new PlaylistModelMapper(_context)));
        CommentRepository = new Lazy<ICommentRepository>(() =>
            new CommentRepository(_context, new CommentMapper(), new CommentModelMapper(_context)));
        RatingRepository =
            new Lazy<IRatingRepository>(() =>
                new RatingRepository(_context, new RatingMapper(), new RatingModelMapper(_context)));
    }

    public Lazy<IUserRepository> UserRepository { get; }

    public Lazy<IFilmRepository> FilmRepository { get; }

    public Lazy<IServerRepository> ServerRepository { get; }

    public Lazy<IRoomRepository> RoomRepository { get; }

    public Lazy<IPlaylistRepository> PlaylistRepository { get; }
    public Lazy<ICommentRepository> CommentRepository { get; }

    public Lazy<IRatingRepository> RatingRepository { get; }


    public async Task SaveChangesAsync()
    {
        foreach (var notification in _context.Notifications.ToList())
        {
            await _mediator.Publish(notification);
        }
        
        await _context.SaveChangesAsync();
    }
}