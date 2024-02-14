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
        UserRepository = new Lazy<IUserRepository>(() =>
            new UserRepository(_context, new UserMapper(), new UserModelMapper(_context)));
        FilmRepository = new Lazy<IFilmRepository>(() =>
            new FilmRepository(_context, new FilmMapper(), new FilmModelMapper(_context)));
    }

    public Lazy<IUserRepository> UserRepository { get; }

    public Lazy<IFilmRepository> FilmRepository { get; }

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