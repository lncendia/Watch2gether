using Overoom.Domain.Film.Enums;

namespace Overoom.Application.Abstractions.Film.DTOs.FilmLoader;

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