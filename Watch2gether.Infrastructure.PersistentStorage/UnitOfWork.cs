using Watch2gether.Domain.Abstractions.Repositories;
using Watch2gether.Domain.Abstractions.Repositories.UnitOfWorks;
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
        FilmRoomRepository = new Lazy<IFilmRoomRepository>(() => new FilmRoomRepository(context));
        YoutubeRoomRepository = new Lazy<IYoutubeRoomRepository>(() => new YoutubeRoomRepository(context));
        PlaylistRepository = new Lazy<IPlaylistRepository>(() => new PlaylistRepository(context));
        CommentRepository = new Lazy<ICommentRepository>(() => new CommentRepository(context));
    }

    public Lazy<IFilmRepository> FilmRepository { get; }
    public Lazy<IUserRepository> UserRepository { get; }
    public Lazy<IFilmRoomRepository> FilmRoomRepository { get; }
    public Lazy<IYoutubeRoomRepository> YoutubeRoomRepository { get; }
    public Lazy<IPlaylistRepository> PlaylistRepository { get; }
    public Lazy<ICommentRepository> CommentRepository { get; }

    public Task SaveAsync() => _context.SaveChangesAsync();
}