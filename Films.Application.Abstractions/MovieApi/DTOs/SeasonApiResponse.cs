namespace Films.Application.Abstractions.MovieApi.DTOs;

public class SeasonApiResponse
{
    public required int Number { get; init; }
    public required IReadOnlyCollection<EpisodeApiResponse> Episodes { get; init; }
}