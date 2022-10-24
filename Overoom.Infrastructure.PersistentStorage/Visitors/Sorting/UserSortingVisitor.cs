using Overoom.Infrastructure.PersistentStorage.Models.Users;
using Overoom.Infrastructure.PersistentStorage.Visitors.Sorting.Models;
using Overoom.Domain.Ordering.Abstractions;
using Overoom.Domain.Users;
using Overoom.Domain.Users.Ordering.Visitor;
using Overoom.Infrastructure.PersistentStorage.Models;

namespace Overoom.Infrastructure.PersistentStorage.Visitors.Sorting;

public class UserSortingVisitor : BaseSortingVisitor<UserModel, IUserSortingVisitor, User>, IUserSortingVisitor
{
    protected override List<SortData<UserModel>> ConvertOrderToList(IOrderBy<User, IUserSortingVisitor> spec)
    {
        var visitor = new UserSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }
}