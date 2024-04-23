using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Servers;
using Films.Domain.Servers.Ordering.Visitor;
using Films.Domain.Servers.Specifications.Visitor;

namespace Films.Domain.Abstractions.Repositories;

public interface IServerRepository : IRepository<Server, Guid, IServerSpecificationVisitor, IServerSortingVisitor>;