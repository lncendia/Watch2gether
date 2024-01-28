using Films.Domain.Ordering.Abstractions;
using Films.Domain.Users.Entities;

namespace Films.Domain.Users.Ordering.Visitor;

public interface IUserSortingVisitor : ISortingVisitor<IUserSortingVisitor, User>
{
}