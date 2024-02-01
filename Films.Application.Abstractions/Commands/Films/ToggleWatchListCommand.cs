using MediatR;

namespace Films.Application.Abstractions.Commands.Films;

public class ToggleWatchListCommand : IRequest
{
    public required Guid FilmId { get; init; }
    public required Guid UserId { get; init; }
}