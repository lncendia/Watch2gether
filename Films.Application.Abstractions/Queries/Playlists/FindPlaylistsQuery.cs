using Films.Application.Abstractions.DTOs.Common;
using Films.Application.Abstractions.DTOs.Playlists;
using MediatR;

namespace Films.Application.Abstractions.Queries.Playlists;

public class FindPlaylistsQuery : IRequest<ListDto<PlaylistDto>>
{
    public string? Query { get; init; }
    public string? Genre { get; init; }
    public Guid? FilmId { get; init; }
    public required int Skip { get; init; }
    public required int Take { get; init; }
}