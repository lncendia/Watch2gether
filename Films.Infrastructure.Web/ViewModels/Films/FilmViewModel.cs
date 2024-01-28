using Films.Domain.Films.Enums;

namespace Films.Infrastructure.Web.Models.Films;

public class FilmViewModel
{
    public FilmViewModel(Guid id, string name, Uri posterUri, double rating, string description, int year,
        FilmType type, int? countSeasons, IEnumerable<string> genres)
    {
        Id = id;
        Name = name + " (" + year + ")";
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
}