using Watch2gether.Domain.Films.Enums;

namespace Watch2gether.WEB.Models.Film;

public class FilmLiteViewModel
{
    public FilmLiteViewModel(Guid id, string name, string posterFileName, double rating, string description, int year,
        FilmType type, int? countSeasons, string genres)
    {
        Id = id;
        Name = name + " (" + year + ")";
        PosterFileName = posterFileName;
        Rating = rating;
        Description = description;
        Type = type switch
        {
            FilmType.Serial => $"Сериал, {countSeasons} сезон(ов)",
            FilmType.Film => "Фильм",
            _ => throw new NotImplementedException()
        };
        Genres = genres;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string PosterFileName { get; }
    public double Rating { get; }
    public string Description { get; }
    public string Type { get; }
    public string Genres { get; }
}