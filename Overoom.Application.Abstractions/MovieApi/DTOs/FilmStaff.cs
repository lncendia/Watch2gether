namespace Overoom.Application.Abstractions.MovieApi.DTOs;

public class FilmStaff
{
    public FilmStaff(IReadOnlyCollection<string> directors, IReadOnlyCollection<string> screenWriters,
        IReadOnlyCollection<(string name, string? desc)> actors)
    {
        Directors = directors;
        ScreenWriters = screenWriters;
        Actors = actors;
    }

    public IReadOnlyCollection<string> Directors { get; }
    public IReadOnlyCollection<string> ScreenWriters { get; }
    public IReadOnlyCollection<(string name, string? desc)> Actors { get; }
}