using Overoom.Domain.Films.Enums;

namespace Overoom.Application.Abstractions.PlaylistsManagement.DTOs;

public class PlaylistShortDto
{
    public PlaylistShortDto(string name, int year, FilmType type, Uri posterUri, Guid id)
    {
        Name = name;
        Year = year;
        Type = type;
        PosterUri = posterUri;
        Id = id;
    }

    public Guid Id { get; }
    public FilmType Type { get; }
    public Uri PosterUri { get; }
    public string Name { get; }
    public int Year { get; }
}