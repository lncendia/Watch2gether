namespace Films.Application.Abstractions.Queries.Ratings.DTOs;

public class RatingDto
{
    public required Guid FilmId { get; init; }
    public required string Name { get; init; }
    public required int Year { get; init; }
    public required Uri PosterUrl { get; init; }
    public required double Score { get; init; }
}