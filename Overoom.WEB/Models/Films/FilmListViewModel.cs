namespace Overoom.WEB.Models.Films;

public class FilmListViewModel
{
    public FilmListViewModel(bool isAdmin, IReadOnlyCollection<FilmShortViewModel> films)
    {
        IsAdmin = isAdmin;
        Films = films;
    }

    public bool IsAdmin { get; }
    public IReadOnlyCollection<FilmShortViewModel> Films { get; }
}