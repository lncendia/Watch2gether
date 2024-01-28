namespace Films.Infrastructure.Web.Models.Film;

public class FilmPageViewModel
{
    public FilmPageViewModel(FilmViewModel film, IReadOnlyCollection<PlaylistShortViewModel> playlists)
    {
        Film = film;
        Playlists = playlists;
    }

    public FilmViewModel Film { get; }
    public IReadOnlyCollection<PlaylistShortViewModel> Playlists { get; }
}