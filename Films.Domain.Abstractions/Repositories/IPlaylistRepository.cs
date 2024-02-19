using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Playlists;
using Films.Domain.Playlists.Ordering.Visitor;
using Films.Domain.Playlists.Specifications.Visitor;

namespace Films.Domain.Abstractions.Repositories;

public interface IPlaylistRepository : IRepository<Playlist, Guid, IPlaylistSpecificationVisitor, IPlaylistSortingVisitor>;