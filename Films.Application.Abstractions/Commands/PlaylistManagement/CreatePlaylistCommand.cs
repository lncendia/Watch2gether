using MediatR;

namespace Films.Application.Abstractions.Commands.PlaylistManagement;

public class CreatePlaylistCommand : IRequest<Guid>
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public Uri? PosterUrl { get; init; }
    public string? PosterBase64 { get; init; }
}