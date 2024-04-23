using MediatR;

namespace Films.Application.Abstractions.Commands.FilmsManagement;

public class DeleteFilmCommand : IRequest
{
    public required Guid Id { get; init; }
}