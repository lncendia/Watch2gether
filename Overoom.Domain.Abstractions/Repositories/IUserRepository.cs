using Overoom.Domain.Abstractions.Interfaces;
using Overoom.Domain.Users;
using Overoom.Domain.Users.Ordering.Visitor;
using Overoom.Domain.Users.Specifications.Visitor;

namespace Overoom.Domain.Abstractions.Repositories;

public interface IUserRepository : IRepository<User, Guid, IUserSpecificationVisitor, IUserSortingVisitor>
{
}