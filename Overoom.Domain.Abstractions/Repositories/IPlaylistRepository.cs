using Overoom.Domain.Abstractions.Interfaces;
using Overoom.Domain.Playlists;
using Overoom.Domain.Playlists.Ordering.Visitor;
using Overoom.Domain.Playlists.Specifications.Visitor;

namespace Overoom.Domain.Abstractions.Repositories;

public interface IPlaylistRepository : IRepository<Playlist, Guid, IPlaylistSpecificationVisitor, IPlaylistSortingVisitor>
{ }