namespace Films.Application.Abstractions.Services.MovieApi.DTOs;

public class FilmShortApiResponse
{
    public required long KpId { get; init; }
    public string? ImdbId { get; init; }
    public required string Title { get; init; }
}