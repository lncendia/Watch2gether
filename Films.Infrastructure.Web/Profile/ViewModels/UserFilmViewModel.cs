namespace Films.Infrastructure.Web.Profile.ViewModels;

public class UserFilmViewModel
{
    public required Guid FilmId { get; init; }
    public required string Title { get; init; }
    public required int Year { get; init; }
    public required string PosterUrl { get; init; }
    public double? RatingKp { get; init; }
    public double? RatingImdb { get; init; }
}