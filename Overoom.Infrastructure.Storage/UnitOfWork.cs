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
    }

    public Lazy<IUserRepository> UserRepository => new(() =>
        new UserRepository(_context, new UserMapper(), new UserModelMapper(_context)));
    public Lazy<IFilmRepository> FilmRepository  => new(() =>
        new FilmRepository(_context, new FilmMapper(), new FilmModelMapper(_context)));
    public Lazy<IFilmRoomRepository> FilmRoomRepository  => new(() =>
        new FilmRoomRepository(_context, new FilmRoomMapper(), new FilmRoomModelMapper(_context)));
    public Lazy<IYoutubeRoomRepository> YoutubeRoomRepository  => new(() =>
        new YoutubeRoomRepository(_context, new YoutubeRoomMapper(), new YoutubeRoomModelMapper(_context)));
    public Lazy<IPlaylistRepository> PlaylistRepository  => new(() =>
        new PlaylistRepository(_context, new PlaylistMapper(), new PlaylistModelMapper(_context)));
    public Lazy<ICommentRepository> CommentRepository  => new(() =>
        new CommentRepository(_context, new CommentMapper(), new CommentModelMapper(_context)));
    public Lazy<IRatingRepository> RatingRepository  => new(() =>
        new RatingRepository(_context, new RatingMapper(), new RatingModelMapper(_context)));
    

    public async Task SaveChangesAsync()
    {
        foreach (var notification in _context.Notifications.ToList())
        {
            await _mediator.Publish(notification);
        }

        await _context.SaveChangesAsync();
    }
}