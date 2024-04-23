using Films.Domain.Ordering.Abstractions;

namespace Films.Domain.Users.Ordering.Visitor;

public interface IUserSortingVisitor : ISortingVisitor<IUserSortingVisitor, User>
{
}