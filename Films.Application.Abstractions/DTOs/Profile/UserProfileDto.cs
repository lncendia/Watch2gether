using Films.Domain.Users.ValueObjects;

namespace Films.Application.Abstractions.DTOs.Profile;

public class UserProfileDto
{
    public required string Username { get; init; }
    public required Uri? PhotoUrl { get; init; }
    public required Allows Allows { get; init; }
    public required IReadOnlyCollection<UserFilmDto> History { get; init; }
    public required IReadOnlyCollection<UserFilmDto> Watchlist { get; init; }
    public required IReadOnlyCollection<string> Genres { get; init; }
}