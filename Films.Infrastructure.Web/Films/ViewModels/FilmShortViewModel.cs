namespace Films.Infrastructure.Web.Films.ViewModels;

public class FilmShortViewModel
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required Uri PosterUrl { get; init; }
    public required double Rating { get; init; }
    public required string Description { get; init; }
    public required string Type { get; init; }

    public int? CountSeasons { get; init; }
    public required IEnumerable<string> Genres { get; init; }
}