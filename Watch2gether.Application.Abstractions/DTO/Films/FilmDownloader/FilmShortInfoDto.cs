using Watch2gether.Domain.Films.Enums;

namespace Watch2gether.Application.Abstractions.DTO.Films.FilmDownloader;

public class FilmShortInfoDto
{
    public FilmShortInfoDto(string id, string name, int year, FilmType type)
    {
        Name = name;
        Year = year;
        Type = type;
        Id = id;
    }

    public string Id { get; }
    public FilmType Type { get; }
    public string Name { get; }
    public int Year { get; }
}