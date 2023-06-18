using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Users.Entities;

namespace Overoom.Domain.Users.Ordering.Visitor;

public interface IUserSortingVisitor : ISortingVisitor<IUserSortingVisitor, User>
{
}