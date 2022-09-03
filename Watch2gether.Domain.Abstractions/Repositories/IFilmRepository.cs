using Watch2gether.Domain.Abstractions.Interfaces;
using Watch2gether.Domain.Films;
using Watch2gether.Domain.Films.Ordering.Visitor;
using Watch2gether.Domain.Films.Specifications.Visitor;

namespace Watch2gether.Domain.Abstractions.Repositories;

public interface IFilmRepository : IRepository<Film, Guid, IFilmSpecificationVisitor, IFilmSortingVisitor>
{ }