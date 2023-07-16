namespace Overoom.WEB.Models.Settings;

public class FilmViewModel
{
    public FilmViewModel(string name, Guid id, int year, Uri poster)
    {
        Name = name + " (" + year + ")";
        Id = id;
        Poster = poster;
    }

    public string Name { get; }
    public Guid Id { get; }
    public Uri Poster { get; }
}