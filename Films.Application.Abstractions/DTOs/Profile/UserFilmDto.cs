namespace Films.Application.Abstractions.DTOs.Profile;

public class UserFilmDto
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required int Year { get; init; }
    public required Uri PosterUrl { get; init; }

    public double? RatingKp { get; init; }

    public double? RatingImdb { get; init; }
}