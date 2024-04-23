using Films.Application.Abstractions.DTOs.Playlists;
using MediatR;

namespace Films.Application.Abstractions.Queries.Playlists;

public class PlaylistByIdQuery : IRequest<PlaylistDto>
{
    public required Guid Id { get; init; }
}