using Overoom.Domain.Abstractions.Interfaces;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Films.Ordering.Visitor;
using Overoom.Domain.Films.Specifications.Visitor;

namespace Overoom.Domain.Abstractions.Repositories;

public interface IFilmRepository : IRepository<Film, Guid, IFilmSpecificationVisitor, IFilmSortingVisitor>
{ }