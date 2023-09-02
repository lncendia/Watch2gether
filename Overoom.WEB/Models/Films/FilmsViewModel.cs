namespace Overoom.WEB.Models.Films;

public class FilmsViewModel
{
    public FilmsViewModel(IReadOnlyCollection<FilmShortViewModel> popularFilms, IReadOnlyCollection<FilmShortViewModel> bestFilms)
    {
        PopularFilms = popularFilms;
        BestFilms = bestFilms;
    }

    public IReadOnlyCollection<FilmShortViewModel> PopularFilms { get; }
    public IReadOnlyCollection<FilmShortViewModel> BestFilms { get; }
}