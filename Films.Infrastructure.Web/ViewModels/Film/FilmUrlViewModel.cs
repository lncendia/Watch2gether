namespace Films.Infrastructure.Web.Models.Film;

public class FilmUrlViewModel
{
    public FilmUrlViewModel(Uri uri)
    {
        Uri = uri.ToString();
    }

    public string Uri { get; }
}