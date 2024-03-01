namespace Films.Application.Abstractions.MovieApi.DTOs;

public class FilmApiResponse
{
    public required long KpId { get; init; }
    public string? ImdbId { get; init; }
    public required string Title { get; init; }
    public required int Year { get; init; }
    public required bool Serial { get; init; }
    public string? Description { get; init; }
    public string? ShortDescription { get; init; }
    public Uri? PosterUrl { get; init; }
    public double? RatingKp { get; init; }
    public double? RatingImdb { get; init; }
    public required IReadOnlyCollection<string> Countries { get; init; }
    public required IReadOnlyCollection<string> Genres { get; init; }
}