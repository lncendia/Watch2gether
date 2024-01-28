using Films.Domain.Films.Enums;

namespace Films.Application.Abstractions.Queries.Films.DTOs;

public class FilmShortDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required Uri PosterUrl { get; init; }
    public required int Year { get; init; }
    public required double Rating { get; init; }
    public required string ShortDescription { get; init; }
    public required FilmType Type { get; init; }
    public required IReadOnlyCollection<string> Genres { get; init; }
    public required int? CountSeasons { get; init; }
}