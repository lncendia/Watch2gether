using Watch2gether.Domain.Abstractions.Interfaces;
using Watch2gether.Domain.Playlists;
using Watch2gether.Domain.Playlists.Ordering.Visitor;
using Watch2gether.Domain.Playlists.Specifications.Visitor;

namespace Watch2gether.Domain.Abstractions.Repositories;

public interface IPlaylistRepository : IRepository<Playlist, Guid, IPlaylistSpecificationVisitor, IPlaylistSortingVisitor>
{ }