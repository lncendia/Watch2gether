namespace Films.Infrastructure.Web.Models.Home;

public class FilmStartPageViewModel
{
    public FilmStartPageViewModel(string name, Uri posterUri, Guid id, IEnumerable<string> genres)
    {
        Name = name;
        PosterUri = posterUri;
        Id = id;
        Genres = string.Join(", ", genres);
    }

    public Guid Id { get; }
    public string Name { get; }
    public Uri PosterUri { get; }
    public string Genres { get; }
}