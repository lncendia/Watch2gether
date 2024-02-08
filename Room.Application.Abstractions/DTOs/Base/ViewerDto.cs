using Room.Domain.Users.ValueObjects;

namespace Room.Application.Abstractions.DTOs.Base;

public abstract class ViewerDto
{
    public required Guid UserId { get; init; }
    public required string UserName { get; init; }
    public required Uri PhotoUrl { get; init; }
    public required bool Pause { get; init; }
    public required bool FullScreen { get; init; }

    public required bool Online { get; init; }
    public required TimeSpan TimeLine { get; init; }
    public required Allows Allows { get; init; }
}