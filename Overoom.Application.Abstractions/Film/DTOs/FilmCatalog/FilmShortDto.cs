using Overoom.Domain.Film.Enums;

namespace Overoom.Application.Abstractions.Film.DTOs.FilmCatalog;

public class FilmShortDto
{
    public FilmShortDto(Guid id, string name, string posterUri, double rating, string description, int year,
        FilmType type, int? countSeasons, IReadOnlyCollection<string> genres)
    {
        Id = id;
        Name = name;
        PosterUri = posterUri;
        Rating = rating;
        ShortDescription = description;
        Year = year;
        Type = type;
        CountSeasons = countSeasons;
        Genres = genres;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string PosterUri { get; }
    public int Year { get; }
    public double Rating { get; }
    public string ShortDescription { get; }
    public FilmType Type { get; }
    public IReadOnlyCollection<string> Genres { get; }
    public int? CountSeasons { get; }
}