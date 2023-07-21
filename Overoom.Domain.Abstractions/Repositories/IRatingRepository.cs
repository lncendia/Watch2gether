using Overoom.Domain.Abstractions.Interfaces;
using Overoom.Domain.Ratings;
using Overoom.Domain.Ratings.Entities;
using Overoom.Domain.Ratings.Ordering.Visitor;
using Overoom.Domain.Ratings.Specifications.Visitor;

namespace Overoom.Domain.Abstractions.Repositories;

public interface
    IRatingRepository : IRepository<Rating, Guid, IRatingSpecificationVisitor, IRatingSortingVisitor>
{
}