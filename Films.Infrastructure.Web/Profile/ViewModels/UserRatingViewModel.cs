namespace Films.Infrastructure.Web.Profile.ViewModels;

public class UserRatingViewModel : UserFilmViewModel
{
    public required double Score { get; init; }
}