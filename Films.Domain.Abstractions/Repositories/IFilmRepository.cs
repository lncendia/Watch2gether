using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Films;
using Films.Domain.Films.Ordering.Visitor;
using Films.Domain.Films.Specifications.Visitor;

namespace Films.Domain.Abstractions.Repositories;

public interface IFilmRepository : IRepository<Film, Guid, IFilmSpecificationVisitor, IFilmSortingVisitor>;