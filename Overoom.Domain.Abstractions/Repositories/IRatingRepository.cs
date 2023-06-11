using Overoom.Domain.Abstractions.Interfaces;
using Overoom.Domain.Rating.Ordering.Visitor;
using Overoom.Domain.Rating.Specifications.Visitor;

namespace Overoom.Domain.Abstractions.Repositories;

public interface
    IRatingRepository : IRepository<Rating.Rating, Guid, IRatingSpecificationVisitor, IRatingSortingVisitor>
{
}