namespace Overoom.Application.Abstractions.StartPage.DTOs;

public class FilmStartPageDto
{
    public FilmStartPageDto(string name, string posterUrl, Guid id, IReadOnlyCollection<string> genres)
    {
        Name = name;
        PosterUrl = posterUrl;
        Id = id;
        Genres = genres;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string PosterUrl { get; }
    public IReadOnlyCollection<string> Genres { get; }
}