namespace Overoom.Application.Abstractions.Authentication.DTOs;

public class FilmDto
{
    public FilmDto(string name, Guid id, int year, Uri poster)
    {
        Name = name;
        Id = id;
        Year = year;
        Poster = poster;
    }

    public string Name { get; }
    public Guid Id { get; }
    public Uri Poster { get; }
    public int Year { get; }
}