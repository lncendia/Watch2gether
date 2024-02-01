using Films.Domain.Films.Enums;

namespace Films.Infrastructure.Web.FilmsApi.ViewModels;

public class FilmApiViewModel
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
    public required IReadOnlyCollection<CdnViewModel> Cdn { get; init; }
    public required IReadOnlyCollection<string> Countries { get; init; }
    public required IReadOnlyCollection<ActorViewModel> Actors { get; init; }
    public required IReadOnlyCollection<string> Directors { get; init; }
    public required IReadOnlyCollection<string> Genres { get; init; }
    public required IReadOnlyCollection<string> Screenwriters { get; init; }
}