using MediatR;

namespace Films.Application.Abstractions.Commands.Films;

public class AddRatingCommand : IRequest
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required double Score { get; init; }
}