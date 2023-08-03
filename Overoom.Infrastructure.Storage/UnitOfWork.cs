using MediatR;
using Overoom.Domain.Abstractions.Repositories;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Infrastructure.Storage.Context;
using Overoom.Infrastructure.Storage.Mappers.AggregateMappers;
using Overoom.Infrastructure.Storage.Mappers.ModelMappers;
using Overoom.Infrastructure.Storage.Repositories;

namespace Overoom.Infrastructure.Storage;

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
        FilmRoomRepository = new Lazy<IFilmRoomRepository>(() =>
            new FilmRoomRepository(_context, new FilmRoomMapper(), new FilmRoomModelMapper(_context)));
        YoutubeRoomRepository = new Lazy<IYoutubeRoomRepository>(() =>
            new YoutubeRoomRepository(_context, new YoutubeRoomMapper(), new YoutubeRoomModelMapper(_context)));
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

    public Lazy<IFilmRoomRepository> FilmRoomRepository { get; }

    public Lazy<IYoutubeRoomRepository> YoutubeRoomRepository { get; }

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