using MediatR;

namespace Films.Application.Abstractions.Commands.Users;

public class AddToHistoryCommand : IRequest
{
    public required Guid FilmId { get; init; }
    public required Guid UserId { get; init; }
}