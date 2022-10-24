using Overoom.Domain.Films.Enums;

namespace Overoom.Application.Abstractions.DTO.Films.FilmInfoGetter;

public class FilmFullInfoDto
{
    public FilmFullInfoDto(string name, string? description, string? shortDescription, double rating, DateOnly date,
        FilmType type, Uri url, IEnumerable<string> genres, IEnumerable<(string name, string description)> actors,
        IEnumerable<string> countries, IEnumerable<string> directors, IEnumerable<string> screenwriters,
        string avatarUrl, int? countSeasons, int? countEpisodes)
    {
        Name = name;
        Date = date;
        Type = type;
        Url = url;
        AvatarUrl = avatarUrl;
        CountSeasons = countSeasons;
        CountEpisodes = countEpisodes;
        Description = description;
        Rating = rating;
        ShortDescription = shortDescription;
        Genres = genres.ToList();
        Actors = actors.ToList();
        Screenwriters = screenwriters.ToList();
        Countries = countries.ToList();
        Directors = directors.ToList();
    }

    public string? Description { get; }
    public string? ShortDescription { get; }
    public FilmType Type { get; }
    public Uri Url { get; }
    public string Name { get; }
    public DateOnly Date { get; }
    public double Rating { get; }
    public int? CountSeasons { get; }
    public int? CountEpisodes { get; }

    public List<string> Countries { get; }
    public List<(string name, string description)> Actors { get; }
    public List<string> Directors { get; }
    public List<string> Genres { get; }
    public List<string> Screenwriters { get; }

    public string AvatarUrl { get; }
}