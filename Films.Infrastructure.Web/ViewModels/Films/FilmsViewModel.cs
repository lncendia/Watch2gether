namespace Films.Infrastructure.Web.Models.Films;

public class FilmsViewModel
{
    public FilmsViewModel(IReadOnlyCollection<FilmShortViewModel> popularFilms)
    {
        PopularFilms = popularFilms;
    }

    public IReadOnlyCollection<FilmShortViewModel> PopularFilms { get; }
}