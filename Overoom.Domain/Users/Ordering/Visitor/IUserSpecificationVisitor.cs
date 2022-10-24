using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.Users.Ordering.Visitor;

public interface IUserSortingVisitor : ISortingVisitor<IUserSortingVisitor, User>
{
}