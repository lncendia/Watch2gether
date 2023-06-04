using Overoom.Domain.Abstractions.Interfaces;
using Overoom.Domain.User.Ordering.Visitor;
using Overoom.Domain.User.Specifications.Visitor;

namespace Overoom.Domain.Abstractions.Repositories;

public interface IUserRepository : IRepository<User.Entities.User, Guid, IUserSpecificationVisitor, IUserSortingVisitor>
{
}