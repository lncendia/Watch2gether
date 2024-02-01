using MediatR;

namespace Films.Application.Abstractions.Commands.Films;

public class AddRatingCommand : IRequest
{
    public required Guid FilmId { get; init; }
    public required Guid UserId { get; init; }
    public required double Score { get; init; }
}