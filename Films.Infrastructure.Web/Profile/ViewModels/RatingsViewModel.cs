namespace Films.Infrastructure.Web.Profile.ViewModels;

public class RatingsViewModel
{
    public required IEnumerable<UserRatingViewModel> Ratings { get; init; }
    public required int CountPages { get; init; }
}