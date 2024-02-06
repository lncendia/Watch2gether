namespace Films.Infrastructure.Web.User.ViewModels;

public class RatingViewModel
{
    public required Guid FilmId { get; init; }
    public required string Name { get; init; }
    public required int Year { get; init; }
    public required Uri PosterUrl { get; init; }
    public required double Score { get; init; }
}