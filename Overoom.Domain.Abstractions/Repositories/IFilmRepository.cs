using Overoom.Domain.Abstractions.Interfaces;
using Overoom.Domain.Film.Ordering.Visitor;
using Overoom.Domain.Film.Specifications.Visitor;

namespace Overoom.Domain.Abstractions.Repositories;

public interface IFilmRepository : IRepository<Film.Entities.Film, Guid, IFilmSpecificationVisitor, IFilmSortingVisitor>
{ }