namespace Films.Infrastructure.Web.Films.ViewModels;

public class FilmsViewModel
{
    public required IEnumerable<FilmShortViewModel> Films { get; init; }
    public required int CountPages { get; init; }
}