namespace Films.Domain.Abstractions.Repositories.UnitOfWorks;

public interface IUnitOfWork
{
    Lazy<IFilmRepository> FilmRepository { get; }
    Lazy<IUserRepository> UserRepository { get; }
    Lazy<IServerRepository> ServerRepository { get; }
    Lazy<IRoomRepository> RoomRepository { get; }
    Lazy<IPlaylistRepository> PlaylistRepository { get; }
    Lazy<ICommentRepository> CommentRepository { get; }
    Lazy<IRatingRepository> RatingRepository { get; }
    Task SaveChangesAsync();
}