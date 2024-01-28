using Films.Domain.Ordering.Abstractions;
using Films.Domain.Users.Entities;
using Films.Domain.Users.Ordering.Visitor;
using Films.Infrastructure.Storage.Models.User;
using Films.Infrastructure.Storage.Visitors.Sorting.Models;

namespace Films.Infrastructure.Storage.Visitors.Sorting;

public class UserSortingVisitor : BaseSortingVisitor<UserModel, IUserSortingVisitor, User>, IUserSortingVisitor
{
    protected override List<SortData<UserModel>> ConvertOrderToList(IOrderBy<User, IUserSortingVisitor> spec)
    {
        var visitor = new UserSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }
}