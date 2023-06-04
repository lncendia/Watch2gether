namespace Overoom.WEB.Models.Home;

public class FilmStartPageViewModel
{
    public FilmStartPageViewModel(string name, string posterUrl, Guid id, IEnumerable<string> genres)
    {
        Name = name;
        PosterUrl = posterUrl;
        Id = id;
        Genres = string.Join(", ", genres);
    }

    public Guid Id { get; }
    public string Name { get; }
    public string PosterUrl { get; }
    public string Genres { get; }
}