namespace Films.Infrastructure.Web.Playlists.ViewModels;

public class PlaylistsViewModel
{
    public required IEnumerable<PlaylistViewModel> Playlists { get; init; }
    public required int CountPages { get; init; }
}