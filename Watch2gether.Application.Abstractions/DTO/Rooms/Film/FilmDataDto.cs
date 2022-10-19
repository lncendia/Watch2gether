using Watch2gether.Domain.Films.Enums;

namespace Watch2gether.Application.Abstractions.DTO.Rooms.Film;

public class FilmDataDto
{
    public FilmDataDto(Guid id, string name, string url, FilmType type)
    {
        Name = name;
        Url = url;
        Type = type;
        Id = id;
    }

    public Guid Id { get; }

    public string Name { get; }
    public string Url { get; }
    public FilmType Type { get; }
}