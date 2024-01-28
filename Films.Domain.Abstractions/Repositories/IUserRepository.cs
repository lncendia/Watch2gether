using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Users.Entities;
using Films.Domain.Users.Ordering.Visitor;
using Films.Domain.Users.Specifications.Visitor;

namespace Films.Domain.Abstractions.Repositories;

public interface IUserRepository : IRepository<User, Guid, IUserSpecificationVisitor, IUserSortingVisitor>;