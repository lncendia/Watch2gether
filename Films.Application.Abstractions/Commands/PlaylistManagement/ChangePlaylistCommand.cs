using MediatR;

namespace Films.Application.Abstractions.Commands.PlaylistManagement;

public class ChangePlaylistCommand : IRequest
{
    public required Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public Uri? PosterUri { get; init; }
    public Stream? PosterStream { get; init; }
    public IReadOnlyCollection<Guid>? Films { get; init; }
}