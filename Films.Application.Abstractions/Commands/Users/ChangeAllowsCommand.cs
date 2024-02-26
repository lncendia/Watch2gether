using MediatR;

namespace Films.Application.Abstractions.Commands.Users;

public class ChangeAllowsCommand : IRequest
{
    public required Guid UserId { get; init; }
    public required bool Beep { get; init; }
    
    public required bool Scream { get; init; }
    
    public required bool Change { get; init; }
}