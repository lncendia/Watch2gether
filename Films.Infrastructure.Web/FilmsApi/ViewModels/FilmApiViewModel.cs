namespace Films.Infrastructure.Web.FilmsApi.ViewModels;

public class FilmApiViewModel
{
    public string? Description { get; init; }
    public string? ShortDescription { get; init; }
    public required bool IsSerial { get; init; }
    public Uri? PosterUrl { get; init; }
    public required string Title { get; init; }
    public required int Year { get; init; }
    public double? RatingKp { get; init; }
    public double? RatingImdb { get; init; }
    public int? CountSeasons { get; init; }
    public int? CountEpisodes { get; init; }
    public required IEnumerable<CdnApiViewModel> Cdn { get; init; }
    public required IEnumerable<string> Countries { get; init; }
    public required IEnumerable<ActorApiViewModel> Actors { get; init; }
    public required IEnumerable<string> Directors { get; init; }
    public required IEnumerable<string> Genres { get; init; }
    public required IEnumerable<string> Screenwriters { get; init; }
}