namespace Films.Application.Abstractions.MovieApi.DTOs;

public class FilmStaffApiResponse
{
    public required IReadOnlyCollection<string> Directors { get; init; }
    public required IReadOnlyCollection<string> ScreenWriters { get; init; }
    public required IReadOnlyCollection<ActorApiResponse> Actors { get; init; }
}