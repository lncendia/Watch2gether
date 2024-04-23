namespace Films.Application.Abstractions.DTOs.Films;

public class FilmShortDto
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required Uri PosterUrl { get; init; }
    public required int Year { get; init; }
    public required double UserRating { get; init; }
    
    public double? RatingKp { get; init; }
    public double? RatingImdb { get; init; }
    public required string Description { get; init; }
    public required bool IsSerial { get; init; }
    public int? CountSeasons { get; init; }
    public int? CountEpisodes { get; init; }
    
    public required IReadOnlyCollection<string> Genres { get; init; }
}