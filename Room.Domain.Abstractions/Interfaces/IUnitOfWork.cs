using Room.Domain.Abstractions.Repositories;

namespace Room.Domain.Abstractions.Interfaces;

public interface IUnitOfWork
{
    Lazy<IUserRepository> UserRepository { get; }
    Lazy<IFilmRoomRepository> FilmRoomRepository { get; }
    Lazy<IYoutubeRoomRepository> YoutubeRoomRepository { get; }
    Lazy<IFilmRepository> FilmRepository { get; }
    Task SaveChangesAsync();
}