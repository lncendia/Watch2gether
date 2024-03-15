using MediatR;
using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Abstractions.Repositories;
using Room.Infrastructure.Storage.Context;
using Room.Infrastructure.Storage.Mappers.AggregateMappers;
using Room.Infrastructure.Storage.Mappers.ModelMappers;
using Room.Infrastructure.Storage.Repositories;

namespace Room.Infrastructure.Storage;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IMediator _mediator;

    public UnitOfWork(ApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
        
        FilmRoomRepository = new Lazy<IFilmRoomRepository>(() =>
            new FilmRoomRepository(_context, new FilmRoomMapper(), new FilmRoomModelMapper(_context)));
        YoutubeRoomRepository = new Lazy<IYoutubeRoomRepository>(() =>
            new YoutubeRoomRepository(_context, new YoutubeRoomMapper(), new YoutubeRoomModelMapper(_context)));
        FilmMessageRepository = new Lazy<IFilmMessageRepository>(() =>
            new FilmMessageRepository(_context, new FilmMessageMapper(), new FilmMessageModelMapper(_context)));
        YoutubeMessageRepository = new Lazy<IYoutubeMessageRepository>(() =>
            new YoutubeMessageRepository(_context, new YoutubeMessageMapper(), new YoutubeMessageModelMapper(_context)));

    }

    public Lazy<IFilmRoomRepository> FilmRoomRepository { get; }
    public Lazy<IYoutubeRoomRepository> YoutubeRoomRepository { get; }
    public Lazy<IFilmMessageRepository> FilmMessageRepository { get; }
    public Lazy<IYoutubeMessageRepository> YoutubeMessageRepository { get; }

    public async Task SaveChangesAsync()
    {
        foreach (var notification in _context.Notifications.ToList())
        {
            await _mediator.Publish(notification);
        }

        await _context.SaveChangesAsync();
    }
}