namespace Overoom.Application.Abstractions.PlaylistsManagement.DTOs;

public class FilmDto
{
    public FilmDto(string name, string description, int year, Uri posterUri, Guid id)
    {
        Name = name;
        Description = description;
        Year = year;
        PosterUri = posterUri;
        Id = id;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public int Year { get; }
    public Uri PosterUri { get; }
}