namespace Films.Infrastructure.Web.User.ViewModels;

public class RatingsViewModel
{
    public required IEnumerable<RatingViewModel> Ratings { get; init; }
    public required int CountPages { get; init; }
}