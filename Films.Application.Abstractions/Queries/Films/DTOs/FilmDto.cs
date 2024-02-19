using Films.Domain.Films.ValueObjects;

namespace Films.Application.Abstractions.Queries.Films.DTOs;

public class FilmDto
{
    public required Guid Id { get; init; }
    public required string Description { get; init; }
    public required bool IsSerial { get; init; }
    public required string Title { get; init; }
    public required Uri PosterUrl { get; init; }
    public required int Year { get; init; }
    public double? RatingKp { get; init; }
    public double? RatingImdb { get; init; }
    public required double UserRating { get; init; }
    public required int UserRatingsCount { get; init; }
    public double? UserScore { get; init; }
    public bool? InWatchlist { get; init; }
    public required IReadOnlyCollection<Cdn> CdnList { get; init; }
    public int? CountSeasons { get; init; }
    public int? CountEpisodes { get; init; }
    public required IReadOnlyCollection<string> Genres { get; init; }
    public required IReadOnlyCollection<string> Countries { get; init; }
    public required IReadOnlyCollection<string> Directors { get; init; }
    public required IReadOnlyCollection<string> ScreenWriters { get; init; }
    public required IReadOnlyCollection<Actor> Actors { get; init; }
}