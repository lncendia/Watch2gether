namespace Films.Infrastructure.Web.Models.FilmManagement;

public class FilmViewModel
{
    public FilmViewModel(Guid id, Uri posterUri, string name, int year)
    {
        Id = id;
        PosterUri = posterUri;
        Name = name + " (" + year + ")";
    }

    public Guid Id { get; }
    public Uri PosterUri { get; }
    public string Name { get; }
}