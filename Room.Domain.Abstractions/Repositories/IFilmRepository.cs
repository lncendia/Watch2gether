using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Films.Entities;
using Room.Domain.Users.Entities;

namespace Room.Domain.Abstractions.Repositories;

public interface IFilmRepository : IRepository<Film, Guid>;