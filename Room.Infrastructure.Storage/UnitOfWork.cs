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

    }

    public Lazy<IFilmRoomRepository> FilmRoomRepository { get; }
    public Lazy<IYoutubeRoomRepository> YoutubeRoomRepository { get; }

    public async Task SaveChangesAsync()
    {
        foreach (var notification in _context.Notifications.ToList())
        {
            await _mediator.Publish(notification);
        }

        await _context.SaveChangesAsync();
    }
}