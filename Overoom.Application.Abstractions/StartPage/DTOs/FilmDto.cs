namespace Overoom.Application.Abstractions.StartPage.DTOs;

public class FilmDto
{
    public FilmDto(string name, Uri posterUrl, Guid id, IReadOnlyCollection<string> genres)
    {
        Name = name;
        PosterUrl = posterUrl;
        Id = id;
        Genres = genres;
    }

    public Guid Id { get; }
    public string Name { get; }
    public Uri PosterUrl { get; }
    public IReadOnlyCollection<string> Genres { get; }
}