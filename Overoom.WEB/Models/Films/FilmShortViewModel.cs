namespace Overoom.WEB.Models.Films;

public class FilmShortViewModel
{
    public FilmShortViewModel(Guid id, string name, Uri posterUri, int year)
    {
        Id = id;
        Name = name + " (" + year + ")";
        PosterUri = posterUri;
    }

    public Guid Id { get; }
    public string Name { get; }
    public Uri PosterUri { get; }
}