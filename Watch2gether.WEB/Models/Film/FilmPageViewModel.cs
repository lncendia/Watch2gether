namespace Watch2gether.WEB.Models.Film;

public class FilmPageViewModel
{
    public FilmPageViewModel(FilmViewModel film, List<PlaylistLiteViewModel> playlists)
    {
        Film = film;
        Playlists = playlists;
    }

    public FilmViewModel Film { get; }
    public List<PlaylistLiteViewModel> Playlists { get; }
}