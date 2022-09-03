using Watch2gether.Domain.Ordering.Abstractions;

namespace Watch2gether.Domain.Users.Ordering.Visitor;

public interface IUserSortingVisitor : ISortingVisitor<IUserSortingVisitor, User>
{
}