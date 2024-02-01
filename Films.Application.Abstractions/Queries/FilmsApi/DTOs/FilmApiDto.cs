using Films.Domain.Films.Enums;
using Films.Domain.Films.ValueObjects;

namespace Films.Application.Abstractions.Queries.FilmsApi.DTOs;

public class FilmApiDto
{
    public string? Description { get; init; }
    public string? ShortDescription { get; init; }
    public required FilmType Type { get; init; }
    public Uri? PosterUrl { get; init; }
    public required string Title { get; init; }
    public required int Year { get; init; }
    public double? RatingKp { get; init; }
    public double? RatingImdb { get; init; }
    public int? CountSeasons { get; init; }
    public int? CountEpisodes { get; init; }
    public required IReadOnlyCollection<Cdn> Cdn { get; init; }
    public required IReadOnlyCollection<string> Countries { get; init; }
    public required IReadOnlyCollection<Actor> Actors { get; init; }
    public required IReadOnlyCollection<string> Directors { get; init; }
    public required IReadOnlyCollection<string> Genres { get; init; }
    public required IReadOnlyCollection<string> Screenwriters { get; init; }
}