using Overoom.Domain.Abstractions.Repositories;
using Overoom.Domain.Abstractions.Repositories.UnitOfWorks;
using Overoom.Infrastructure.Storage.Context;
using Overoom.Infrastructure.Storage.Repositories;

namespace Overoom.Infrastructure.Storage;

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