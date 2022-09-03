using Watch2gether.Domain.Abstractions.Interfaces;
using Watch2gether.Domain.Users;
using Watch2gether.Domain.Users.Ordering.Visitor;
using Watch2gether.Domain.Users.Specifications.Visitor;

namespace Watch2gether.Domain.Abstractions.Repositories;

public interface IUserRepository : IRepository<User, Guid, IUserSpecificationVisitor, IUserSortingVisitor>
{
}