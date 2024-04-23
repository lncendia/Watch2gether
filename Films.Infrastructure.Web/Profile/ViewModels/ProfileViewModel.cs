namespace Films.Infrastructure.Web.Profile.ViewModels;

public class ProfileViewModel
{
    public required AllowsViewModel Allows { get; init; }
    public required IEnumerable<UserFilmViewModel> Watchlist { get; init; }
    public required IEnumerable<UserFilmViewModel> History { get; init; }
    public required IEnumerable<string> Genres { get; init; }
}