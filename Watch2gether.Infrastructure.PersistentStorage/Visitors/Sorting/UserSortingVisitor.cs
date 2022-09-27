using Watch2gether.Domain.Ordering.Abstractions;
using Watch2gether.Domain.Users;
using Watch2gether.Domain.Users.Ordering.Visitor;
using Watch2gether.Infrastructure.PersistentStorage.Models;
using Watch2gether.Infrastructure.PersistentStorage.Models.Users;
using Watch2gether.Infrastructure.PersistentStorage.Visitors.Sorting.Models;

namespace Watch2gether.Infrastructure.PersistentStorage.Visitors.Sorting;

public class UserSortingVisitor : BaseSortingVisitor<UserModel, IUserSortingVisitor, User>, IUserSortingVisitor
{
    protected override List<SortData<UserModel>> ConvertOrderToList(IOrderBy<User, IUserSortingVisitor> spec)
    {
        var visitor = new UserSortingVisitor();
        spec.Accept(visitor);
        return visitor.SortItems;
    }
}