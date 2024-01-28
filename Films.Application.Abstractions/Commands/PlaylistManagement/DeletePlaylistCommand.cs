using MediatR;

namespace Films.Application.Abstractions.Commands.PlaylistManagement;

public class DeletePlaylistCommand : IRequest
{
    public required Guid Id { get; init; }
}