using Watch2gether.Domain.Abstractions.Repositories;
using Watch2gether.Infrastructure.PersistentStorage.Context;
using Watch2gether.Infrastructure.PersistentStorage.Repositories;

namespace Watch2gether.Infrastructure.PersistentStorage;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        UserRepository = new Lazy<IUserRepository>(() => new UserRepository(context));
        FilmRepository = new Lazy<IFilmRepository>(() => new FilmRepository(context));
        RoomRepository = new Lazy<IRoomRepository>(() => new RoomRepository(context));
        PlaylistRepository = new Lazy<IPlaylistRepository>(() => new PlaylistRepository(context));
    }

    public Lazy<IFilmRepository> FilmRepository { get; }
    public Lazy<IUserRepository> UserRepository { get; }
    public Lazy<IRoomRepository> RoomRepository { get; }
    public Lazy<IPlaylistRepository> PlaylistRepository { get; }

    public Task SaveAsync() => _context.SaveChangesAsync();
}