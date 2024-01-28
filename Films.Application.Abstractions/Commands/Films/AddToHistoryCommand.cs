using MediatR;

namespace Films.Application.Abstractions.Commands.Films;

public class AddToHistoryCommand : IRequest
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
}