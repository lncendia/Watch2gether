using MediatR;

namespace Films.Application.Abstractions.Commands.PlaylistManagement;

public class ChangePlaylistCommand : IRequest
{
    public required Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public Uri? PosterUrl { get; init; }
    public string? PosterBase64 { get; init; }
    public IReadOnlyCollection<Guid>? Films { get; init; }
}