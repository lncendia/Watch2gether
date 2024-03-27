using Films.Domain.Films.ValueObjects;

namespace Films.Application.Abstractions.DTOs.Films;

public class FilmDto : FilmShortDto
{
    public required int UserRatingsCount { get; init; }
    public double? UserScore { get; init; }
    public bool? InWatchlist { get; init; }
    public required IReadOnlyCollection<Cdn> CdnList { get; init; }
    public required IReadOnlyCollection<string> Countries { get; init; }
    public required IReadOnlyCollection<string> Directors { get; init; }
    public required IReadOnlyCollection<string> ScreenWriters { get; init; }
    public required IReadOnlyCollection<Actor> Actors { get; init; }
}