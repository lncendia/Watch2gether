using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Ratings;
using Films.Domain.Ratings.Entities;
using Films.Domain.Ratings.Ordering.Visitor;
using Films.Domain.Ratings.Specifications.Visitor;

namespace Films.Domain.Abstractions.Repositories;

public interface
    IRatingRepository : IRepository<Rating, Guid, IRatingSpecificationVisitor, IRatingSortingVisitor>;