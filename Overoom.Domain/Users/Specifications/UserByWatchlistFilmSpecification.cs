using Overoom.Domain.Specifications.Abstractions;
using Overoom.Domain.Users.Entities;
using Overoom.Domain.Users.Specifications.Visitor;

namespace Overoom.Domain.Users.Specifications;

public class UserByWatchlistFilmSpecification : ISpecification<User, IUserSpecificationVisitor>
{
    public UserByWatchlistFilmSpecification(Guid id) => Id = id;
    public Guid Id { get; }

    public bool IsSatisfiedBy(User item) => item.Watchlist.Contains(Id);

    public void Accept(IUserSpecificationVisitor visitor) => visitor.Visit(this);
}