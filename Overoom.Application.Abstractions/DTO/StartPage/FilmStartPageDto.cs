namespace Overoom.Application.Abstractions.DTO.StartPage;

public class FilmStartPageDto
{
    public FilmStartPageDto(string name, string posterUrl, Guid id, List<string> genres)
    {
        Name = name;
        PosterUrl = posterUrl;
        Id = id;
        Genres = genres;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string PosterUrl { get; }
    public List<string> Genres { get; }
}