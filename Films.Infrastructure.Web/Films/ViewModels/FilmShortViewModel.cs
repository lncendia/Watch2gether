namespace Films.Infrastructure.Web.Films.ViewModels;

public class FilmShortViewModel
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string PosterUrl { get; init; }

    public double? RatingKp { get; init; }
    public double? RatingImdb { get; init; }
    public required double UserRating { get; init; }
    public required string Description { get; init; }
    public required bool IsSerial { get; init; }

    public int? CountSeasons { get; init; }
    public int? CountEpisodes { get; init; }
    public required IEnumerable<string> Genres { get; init; }
}