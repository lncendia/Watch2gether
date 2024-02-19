using Room.Domain.Abstractions.Repositories;

namespace Room.Domain.Abstractions.Interfaces;

public interface IUnitOfWork
{
    Lazy<IFilmRoomRepository> FilmRoomRepository { get; }
    Lazy<IYoutubeRoomRepository> YoutubeRoomRepository { get; }
    Task SaveChangesAsync();
}