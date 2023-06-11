using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.User.Entities;
using Overoom.Domain.User.Ordering.Visitor;
using Overoom.Infrastructure.Storage.Models.User;
using Overoom.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Overoom.Infrastructure.Storage.Visitors.Sorting;

public class UserSortingVisitor : BaseSortingVisitor<UserModel, IUserSortingVisitor, User>, IUserSortingVisitor
{
    protected override List<SortData<UserModel>> ConvertOrderToList(IOrderBy<User, IUserSortingVisitor> spec)
    {
        var visitor = new UserSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }
}