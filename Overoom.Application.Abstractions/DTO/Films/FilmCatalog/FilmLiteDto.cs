using Overoom.Domain.Films.Enums;

namespace Overoom.Application.Abstractions.DTO.Films.FilmCatalog;

public class FilmLiteDto
{
    public FilmLiteDto(Guid id, string name, string posterFileName, double rating, string description, int year, FilmType type, int? countSeasons, List<string> genres)
    {
        Id = id;
        Name = name;
        PosterFileName = posterFileName;
        Rating = rating;
        ShortDescription = description;
        Year = year;
        Type = type;
        CountSeasons = countSeasons;
        Genres = genres;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string PosterFileName { get; }
    public int Year { get; }
    public double Rating { get; }
    public string ShortDescription { get; }
    public FilmType Type { get; }
    public List<string> Genres { get; }
    public int? CountSeasons { get; }
}