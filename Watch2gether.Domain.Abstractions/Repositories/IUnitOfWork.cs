namespace Watch2gether.Domain.Abstractions.Repositories;

public interface IUnitOfWork
{
    Lazy<IFilmRepository> FilmRepository { get; }
    Lazy<IUserRepository> UserRepository { get; }
    Lazy<IRoomRepository> RoomRepository { get; }
    Lazy<IPlaylistRepository> PlaylistRepository { get; } 
    Task SaveAsync();
}