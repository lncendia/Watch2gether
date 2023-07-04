using Overoom.Domain.Films.Enums;

namespace Overoom.Application.Abstractions.Rooms.DTOs.Film;

public class FilmDataDto
{
    public FilmDataDto(Guid id, string name, Uri url, FilmType type, CdnType cdn)
    {
        Name = name;
        Url = url;
        Type = type;
        Cdn = cdn;
        Id = id;
    }

    public Guid Id { get; }
    public string Name { get; }
    public Uri Url { get; }
    public CdnType Cdn { get; }
    public FilmType Type { get; }
}