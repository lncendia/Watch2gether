using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Rooms.Entities;
using Films.Domain.Rooms.Ordering.Visitor;
using Films.Domain.Rooms.Specifications.Visitor;
using Films.Domain.Servers.Entities;
using Films.Domain.Servers.Ordering.Visitor;
using Films.Domain.Servers.Specifications.Visitor;

namespace Films.Domain.Abstractions.Repositories;

public interface IServerRepository : IRepository<Server, Guid, IServerSpecificationVisitor, IServerSortingVisitor>;