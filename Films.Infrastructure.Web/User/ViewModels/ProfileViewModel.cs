using Films.Infrastructure.Web.User.ViewModels;

namespace Films.Infrastructure.Web.Profile.ViewModels;

public class ProfileViewModel
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required Uri Avatar { get; init; }
    public required AllowsViewModel Allows { get; init; }
    public required IEnumerable<RatingViewModel> WatchedFilms { get; init; }
    public required IEnumerable<RatingViewModel> FavoriteFilms { get; init; }
    public required IEnumerable<string> Genres { get; init; }
}