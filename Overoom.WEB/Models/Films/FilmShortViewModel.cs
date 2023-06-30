using Overoom.Domain.Films.Enums;

namespace Overoom.WEB.Models.Films;

public class FilmShortViewModel
{
    public FilmShortViewModel(Guid id, string name, Uri posterUri, double rating, string description, int year,
        FilmType type, int? countSeasons, IReadOnlyCollection<string> genres)
    {
        Id = id;
        Name = name;
        Year = year;
        PosterUri = posterUri;
        Rating = rating;
        Description = description;
        Type = type switch
        {
            FilmType.Serial => $"Сериал, {countSeasons} сезон(ов)",
            FilmType.Film => "Фильм",
            _ => throw new NotImplementedException()
        };
        Genres = string.Join(", ", genres);
    }

    public Guid Id { get; }
    public string Name { get; }
    public Uri PosterUri { get; }
    public double Rating { get; }
    public string Description { get; }
    public string Type { get; }
    public string Genres { get; }
    public int Year { get; }
}