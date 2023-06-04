using Overoom.Domain.Ordering.Abstractions;

namespace Overoom.Domain.User.Ordering.Visitor;

public interface IUserSortingVisitor : ISortingVisitor<IUserSortingVisitor, User.Entities.User>
{
}