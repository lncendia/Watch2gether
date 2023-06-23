namespace Overoom.WEB.Models.Films;

public class FilmPageViewModel
{
    public FilmPageViewModel(FilmViewModel film, IReadOnlyCollection<PlaylistViewModel> playlists)
    {
        Film = film;
        Playlists = playlists;
    }

    public FilmViewModel Film { get; }
    public IReadOnlyCollection<PlaylistViewModel> Playlists { get; }
}