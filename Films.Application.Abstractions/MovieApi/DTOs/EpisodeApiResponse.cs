namespace Films.Application.Abstractions.MovieApi.DTOs;

public class EpisodeApiResponse
{
    public required int EpisodeNumber { get; init; }
    public required DateOnly? ReleaseDate { get; init; }
}