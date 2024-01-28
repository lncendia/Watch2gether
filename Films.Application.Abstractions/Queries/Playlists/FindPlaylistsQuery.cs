using Films.Application.Abstractions.Queries.Playlists.DTOs;
using MediatR;

namespace Films.Application.Abstractions.Queries.Playlists;

public class FindPlaylistsQuery : IRequest<IReadOnlyCollection<PlaylistDto>>
{
    public string? Query { get; init; }
    public string? Genre { get; init; }
    public required int Skip { get; init; }
    public required int Take { get; init; }
}