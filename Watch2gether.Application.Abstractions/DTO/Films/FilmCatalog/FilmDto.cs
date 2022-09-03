using Watch2gether.Domain.Films.Enums;

namespace Watch2gether.Application.Abstractions.DTO.Films.FilmCatalog;

public class FilmDto
{
    public FilmDto(Guid id, string name, int year, FilmType type, string posterFileName,
        string description, double rating, List<string> directors, List<string> screenWriters, List<string> genres,
        List<string> countries, List<(string name, string desc)> actors, int? countSeasons, int? countEpisodes)
    {
        Name = name;
        Year = year;
        Type = type;
        PosterFileName = posterFileName;
        Description = description;
        Rating = rating;
        Directors = directors;
        Genres = genres;
        Countries = countries;
        Actors = actors;
        CountSeasons = countSeasons;
        CountEpisodes = countEpisodes;
        ScreenWriters = screenWriters;
        Id = id;
    }

    public Guid Id { get; }
    public string Description { get; }
    public FilmType Type { get; }
    public string Name { get; }
    public string PosterFileName { get; }
    public int Year { get; }
    public double Rating { get; }
    public int? CountSeasons { get; }
    public int? CountEpisodes { get; }

    public List<string> Genres { get; }
    public List<string> Countries { get; }
    public List<string> Directors { get; }
    public List<string> ScreenWriters { get; }
    public List<(string name, string desc)> Actors { get; }
}

public class CommentDto
{
    public CommentDto(Guid id, string text, DateTime createdAt, string username)
    {
        Id = id;
        Text = text;
        CreatedAt = createdAt;
        Username = username;
    }

    public Guid Id { get; }
    public string Text { get; }
    public DateTime CreatedAt { get; }
    public string Username { get; }
}