using Room.Domain.Abstractions.Interfaces;
using Room.Domain.Users.Entities;

namespace Room.Domain.Abstractions.Repositories;

public interface IUserRepository : IRepository<User, Guid>;