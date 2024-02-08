using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Rooms.FilmRoom.Entities;

namespace Room.Domain.Abstractions.Repositories;

public interface IFilmRoomRepository : IRepository<FilmRoom, Guid>;