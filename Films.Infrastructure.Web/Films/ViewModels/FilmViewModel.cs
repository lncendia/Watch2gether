namespace Films.Infrastructure.Web.Films.ViewModels;

public class FilmViewModel : FilmShortViewModel
{
    public required double? RatingKp { get; init; }
    public required double? RatingImdb { get; init; }
    public required int UserRatingsCount { get; init; }
    public required double? UserScore { get; init; }
    public required bool? InWatchlist { get; init; }
    public required IEnumerable<CdnViewModel> CdnList { get; init; }
    public required int? CountSeasons { get; init; }
    public required int? CountEpisodes { get; init; }
    public required IEnumerable<string> Countries { get; init; }
    public required IEnumerable<string> Directors { get; init; }
    public required IEnumerable<string> ScreenWriters { get; init; }
    public required IEnumerable<ActorViewModel> Actors { get; init; }
}