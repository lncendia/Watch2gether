using Overoom.Domain.Abstractions.Interfaces;
using Overoom.Domain.Playlist.Ordering.Visitor;
using Overoom.Domain.Playlist.Specifications.Visitor;

namespace Overoom.Domain.Abstractions.Repositories;

public interface IPlaylistRepository : IRepository<Playlist.Entities.Playlist, Guid, IPlaylistSpecificationVisitor, IPlaylistSortingVisitor>
{ }