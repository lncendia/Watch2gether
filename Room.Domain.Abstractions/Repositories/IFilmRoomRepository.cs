using Room.Domain.Abstractions.Interfaces;
using Room.Domain.FilmRooms;

namespace Room.Domain.Abstractions.Repositories;

public interface IFilmRoomRepository : IRepository<FilmRoom, Guid>;