namespace Films.Application.Abstractions.MovieApi.DTOs;

public class CdnApiResponse
{
    public required Uri Url { get; init; }
    public required string Quality { get; init; }
    public required IReadOnlyCollection<string> Voices { get; init; }
}