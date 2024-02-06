using Films.Domain.Users.ValueObjects;

namespace Films.Application.Abstractions.Queries.Users.DTOs;

public class UserProfileDto
{
    public required string UserName { get; init; }
    public required Uri PhotoUrl { get; init; }
    public required Allows Allows { get; init; }
    public required IReadOnlyCollection<string> Genres { get; init; }
}